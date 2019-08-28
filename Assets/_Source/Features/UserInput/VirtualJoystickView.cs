using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.UserInput
{
    public class VirtualJoystickView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, VirtualJoystickView> { }

        [SerializeField] private GameObject _virtualJoystickParent;
        [SerializeField] private Transform _circleParent;
        [SerializeField] private Transform _indicatorParent;

        private IReadOnlyVirtualJoystickModel _virtualJoystickModel;        

        [Inject]
        private void Inject(
            IReadOnlyVirtualJoystickModel virtualJoystickModel)
        {
            _virtualJoystickModel = virtualJoystickModel;
        }

        public void Initialize()
        {
            _virtualJoystickModel.IsPointerDown
                .Subscribe(_virtualJoystickParent.SetActive)
                .AddTo(Disposer);

            _virtualJoystickModel.StartPointerPosition
                .Subscribe(UpdateCirclePosition)
                .AddTo(Disposer);

            _virtualJoystickModel.CurrentPointerPosition
                .Subscribe(UpdateIndicatorPosition)
                .AddTo(Disposer);
        }

        private void UpdateCirclePosition(Vector2 position)
        {
            _circleParent.position = position;
        }

        private void UpdateIndicatorPosition(Vector2 position)
        {
            _indicatorParent.position = position;

            var differenceToCircle = _indicatorParent.position - _circleParent.position;
            var rotation = Quaternion.LookRotation(Vector3.forward, differenceToCircle);

            _indicatorParent.localRotation = rotation;            
        }
    }
}