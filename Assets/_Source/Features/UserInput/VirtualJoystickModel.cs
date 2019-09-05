using _Source.Util;
using UniRx;
using UnityEngine;

namespace _Source.Features.UserInput
{
    public class VirtualJoystickModel : AbstractDisposable, IVirtualJoystickModel
    {
        private readonly ReactiveProperty<bool> _isPointerDown;
        public IReadOnlyReactiveProperty<bool> IsPointerDown => _isPointerDown;

        private readonly ReactiveProperty<Vector2> _startPointerPosition;
        public IReadOnlyReactiveProperty<Vector2> StartPointerPosition => _startPointerPosition;

        private readonly ReactiveProperty<Vector2> _currentPointerPosition;
        public IReadOnlyReactiveProperty<Vector2> CurrentPointerPosition => _currentPointerPosition;

        public VirtualJoystickModel()
        {
            _isPointerDown = new ReactiveProperty<bool>().AddTo(Disposer);
            _startPointerPosition = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(Disposer);
            _currentPointerPosition = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(Disposer);
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
