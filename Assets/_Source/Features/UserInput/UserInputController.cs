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

        private readonly UserInputModel _userInputModel;
        private readonly UserInputConfig _userInputConfig;

        private bool _isDraggingJoystick;
        private Vector2 _startTouchPosition;

        public UserInputController(
            UserInputModel userInputModel,
            UserInputConfig userInputConfig)
        {
            _userInputModel = userInputModel;
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

            _userInputModel.SetInputVector(new Vector2(horizontalAxis, verticalAxis));
        }

        private void TrackPointerInput()
        {
            var isDraggingJoystickInCurrentFrame = IsPointer();
            _userInputModel.SetIsPointerDown(isDraggingJoystickInCurrentFrame);

            if (!_isDraggingJoystick && isDraggingJoystickInCurrentFrame)
            {
                var startPointerPosition = GetPointerPosition();
                _userInputModel.SetStartPointerPosition(startPointerPosition);
            }

            _isDraggingJoystick = isDraggingJoystickInCurrentFrame;

            if (_isDraggingJoystick)
            {
                var currentPointerPosition = GetPointerPosition();
                _userInputModel.SetCurrentPointerPosition(currentPointerPosition);

                var dragDirection = currentPointerPosition - _userInputModel.StartPointerPosition.Value;
                var smoothedDragDirection = GetMagnitudeSmoothedVector(dragDirection);

                _userInputModel.SetInputVector(smoothedDragDirection);
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
