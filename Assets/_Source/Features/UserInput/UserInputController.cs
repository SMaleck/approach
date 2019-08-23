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

            _userInputModel.SetInputVector(horizontalAxis, verticalAxis);
        }

        private void TrackPointerInput()
        {
            var isDraggingJoystickInCurrentFrame = IsPointer();
            
            if (!_isDraggingJoystick && isDraggingJoystickInCurrentFrame)
            {
                _startTouchPosition = GetPointerPosition();
            }

            _isDraggingJoystick = isDraggingJoystickInCurrentFrame;

            if (_isDraggingJoystick)
            {
                var dragDirection = GetPointerPosition() - _startTouchPosition;
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
            var relativeMagnitude = vector.magnitude / _userInputConfig.VirtualJoystickMaxMagnitude;
            return Vector2.ClampMagnitude(vector, relativeMagnitude);
        }
    }
}
