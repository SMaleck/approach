using System;
using _Source.Features.Actors;
using _Source.Features.GameRound;
using _Source.Features.UserInput;
using _Source.Features.UserInput.Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Movement
{
    public class InputMovementController : AbstractMovementController
    {
        public class Factory : PlaceholderFactory<IActorStateModel, InputMovementController> { }

        private const string AxisNameHorizontal = "Horizontal";
        private const string AxisNameVertical = "Vertical";

        private readonly VirtualJoystickModel _virtualJoystickModel;
        private readonly UserInputConfig _userInputConfig;
        private readonly IPauseStateModel _pauseStateModel;

        private bool _isDraggingJoystick;
        private Vector2 _startTouchPosition;

        public InputMovementController(
            IActorStateModel actorStateModel,
            VirtualJoystickModel virtualJoystickModel,
            UserInputConfig userInputConfig,
            IPauseStateModel pauseStateModel)
            : base(actorStateModel)
        {
            _virtualJoystickModel = virtualJoystickModel;
            _userInputConfig = userInputConfig;
            _pauseStateModel = pauseStateModel;

            Observable.EveryFixedUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            if (_pauseStateModel.IsPaused.Value)
            {
                MovementDataComponent.ResetMoveIntention();
                return;
            }

            TrackKeyInput();
            TrackPointerInput();
        }

        private void TrackKeyInput()
        {
            var inputVector = new Vector2(
                Input.GetAxisRaw(AxisNameHorizontal), 
                Input.GetAxisRaw(AxisNameVertical));

            ProcessInputVector(inputVector);
        }

        private void TrackPointerInput()
        {
            var isDraggingJoystickInCurrentFrame = IsPointer();
            _virtualJoystickModel.SetIsPointerDown(isDraggingJoystickInCurrentFrame);

            if (!_isDraggingJoystick && isDraggingJoystickInCurrentFrame)
            {
                var startPointerPosition = GetPointerPosition();
                _virtualJoystickModel.SetStartPointerPosition(startPointerPosition);
            }

            _isDraggingJoystick = isDraggingJoystickInCurrentFrame;

            if (_isDraggingJoystick)
            {
                var currentPointerPosition = GetPointerPosition();
                _virtualJoystickModel.SetCurrentPointerPosition(currentPointerPosition);

                var dragDirection = currentPointerPosition - _virtualJoystickModel.StartPointerPosition.Value;
                var smoothedDragDirection = GetMagnitudeSmoothedVector(dragDirection);

                ProcessInputVector(smoothedDragDirection);
            }
        }

        private bool IsPointer()
        {
            return Input.GetMouseButton(0);
        }

        private Vector2 GetPointerPosition()
        {
            return Input.mousePosition;
        }

        // Magnitude limit seems to be higher for mouse input as opposed to the joysticks max position
        // However it works as expected on a touch input device
        private Vector2 GetMagnitudeSmoothedVector(Vector2 vector)
        {
            var maxMagnitude = _userInputConfig.VirtualJoystickMaxMagnitude;
            vector = Vector2.ClampMagnitude(vector, maxMagnitude);

            // Calculate it down to a relative magnitude, so magnitude is always within [0, 1]
            var relativeMagnitude = vector.magnitude / maxMagnitude;

            return Vector2.ClampMagnitude(vector, relativeMagnitude);
        }

        private void ProcessInputVector(Vector3 input)
        {
            if (!IsProcessable(input))
            {
                return;
            }

            MovementDataComponent.SetMovementIntention(input);

            var turnIntention = Quaternion.LookRotation(Vector3.forward, input);
            MovementDataComponent.SetTurnIntention(turnIntention);
        }

        private bool IsProcessable(Vector2 input)
        {
            return Math.Abs(input.x) <= _userInputConfig.DeadZone &&
                   Math.Abs(input.y) <= _userInputConfig.DeadZone;
        }
    }
}
