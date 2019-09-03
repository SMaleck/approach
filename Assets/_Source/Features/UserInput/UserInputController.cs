using _Source.Features.UserInput.Data;
using _Source.Util;
using Assets._Source.Features.Movement;
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

        private bool _isDraggingJoystick;
        private Vector2 _startTouchPosition;

        public UserInputController(
            MovementModel movementModel,
            VirtualJoystickModel virtualJoystickModel,
            UserInputConfig userInputConfig)
        {
            _movementModel = movementModel;
            _virtualJoystickModel = virtualJoystickModel;
            _userInputConfig = userInputConfig;

            Observable.EveryUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            TrackKeyInput();
            TrackPointerInput();
        }

        private void TrackKeyInput()
        {
            var horizontalAxis = Input.GetAxisRaw(AxisNameHorizontal);
            var verticalAxis = Input.GetAxisRaw(AxisNameVertical);

            var inputVector = new Vector2(horizontalAxis, verticalAxis);
            _movementModel.SetMovementIntention(inputVector);

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

        private Vector2 GetMagnitudeSmoothedVector(Vector2 vector)
        {
            App.Logger.Log($"RAW: {vector} MAG {vector.magnitude}");
            var maxMagnitude = _userInputConfig.VirtualJoystickMaxMagnitude;
            vector = Vector2.ClampMagnitude(vector, maxMagnitude);

            // Calculate it down to a relative magnitude, so magnitude is always within [0, 1]
            var relativeMagnitude = vector.magnitude / maxMagnitude;

            App.Logger.Log($"LiMAG: {vector.magnitude} RelMAG {relativeMagnitude}");
            App.Logger.Log("");

            return Vector2.ClampMagnitude(vector, relativeMagnitude);
        }
    }
}
