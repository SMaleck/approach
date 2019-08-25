using _Source.Features.UserInput.Data;
using _Source.Util;
using UniRx;
using UnityEngine;

namespace _Source.Features.UserInput
{
    public class UserInputModel : AbstractDisposable, IReadOnlyUserInputModel, IReadOnlyVirtualJoystickModel
    {
        private readonly UserInputConfig _userInputConfig;

        private readonly ReactiveProperty<Vector2> _inputVector;
        public IReadOnlyReactiveProperty<Vector2> InputVector => _inputVector;

        public bool HasInput => InputVector.Value.magnitude > _userInputConfig.DeadZone;


        // IReadOnlyVirtualJoystickModel Implementation -----------------------------

        private readonly ReactiveProperty<bool> _isPointerDown;
        public IReadOnlyReactiveProperty<bool> IsPointerDown => _isPointerDown;

        private readonly ReactiveProperty<Vector2> _startPointerPosition;
        public IReadOnlyReactiveProperty<Vector2> StartPointerPosition => _startPointerPosition;

        private readonly ReactiveProperty<Vector2> _currentPointerPosition;
        public IReadOnlyReactiveProperty<Vector2> CurrentPointerPosition => _currentPointerPosition;

        public UserInputModel(UserInputConfig userInputConfig)
        {
            _userInputConfig = userInputConfig;
            _inputVector = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(Disposer);

            _isPointerDown = new ReactiveProperty<bool>().AddTo(Disposer);
            _startPointerPosition = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(Disposer);
            _currentPointerPosition = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(Disposer);
        }

        public void SetInputVector(Vector2 inputVector)
        {
            var isAboveDeadZone = inputVector.magnitude > _userInputConfig.DeadZone;
            inputVector = isAboveDeadZone ? inputVector : Vector2.zero;

            _inputVector.Value = Vector2.ClampMagnitude(inputVector, 1);
        }

        public void SetIsPointerDown(bool value)
        {
            _isPointerDown.Value = value;
        }

        public void SetStartPointerPosition(Vector2 value)
        {
            _startPointerPosition.Value = value;
        }

        public void SetCurrentPointerPosition(Vector2 value)
        {
            _currentPointerPosition.Value = value;
        }
    }
}
