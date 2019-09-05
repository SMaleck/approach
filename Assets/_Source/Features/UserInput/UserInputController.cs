using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Features.UserInput.Data;
using _Source.Util;
using UniRx;
using UnityEngine;

namespace _Source.Features.UserInput
{
    public class UserInputController : AbstractDisposable
    {
        private const string AxisNameHorizontal = "Horizontal";
        private const string AxisNameVertical = "Vertical";

        private readonly MovementModel _movementModel;
        private readonly VirtualJoystickModel _virtualJoystickModel;
        private readonly UserInputConfig _userInputConfig;
        private readonly IPauseStateModel _pauseStateModel;

        private bool _isDraggingJoystick;
        private Vector2 _startTouchPosition;

        public UserInputController(
            MovementModel movementModel,
            VirtualJoystickModel virtualJoystickModel,
            UserInputConfig userInputConfig,
            IPauseStateModel pauseStateModel)
        {
            _movementModel = movementModel;
            _virtualJoystickModel = virtualJoystickModel;
            _userInputConfig = userInputConfig;
            _pauseStateModel = pauseStateModel;

            Observable.EveryUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            if (_pauseStateModel.IsPaused.Value)
            {
                _movementModel.Reset();
                return;
            }

            TrackKeyInput();
            TrackPointerInput();
        }

        private void TrackKeyInput()
        {
            var horizontalAxis = Input.GetAxisRaw(AxisNameHorizontal);
            var verticalAxis = Input.GetAxisRaw(AxisNameVertical);

            var inputVector = new Vector2(horizontalAxis, verticalAxis);
            _movementModel.SetMovementIntention(inputVector);

            // ToDo V2.Zero is a quick hack, need the actual values of the entity
            var heading = inputVector - Vector2.zero;
            var lookRotation = Quaternion.LookRotation(Vector3.forward, heading);
            _movementModel.SetTurnIntention(lookRotation);
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

                _movementModel.SetMovementIntention(smoothedDragDirection);

                // ToDo V2.Zero is a quick hack, need the actual values of the entity
                var heading = smoothedDragDirection - Vector2.zero;
                var lookRotation = Quaternion.LookRotation(Vector3.forward, heading);
                _movementModel.SetTurnIntention(lookRotation);
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
    }
}
