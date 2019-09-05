using _Source.Features.UserInput.Data;
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

        private IVirtualJoystickModel _virtualJoystickModel;
        private UserInputConfig _userInputConfig;

        [Inject]
        private void Inject(
            IVirtualJoystickModel virtualJoystickModel,
            UserInputConfig userInputConfig)
        {
            _virtualJoystickModel = virtualJoystickModel;
            _userInputConfig = userInputConfig;
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
            _indicatorParent.localPosition = Vector2.ClampMagnitude(_indicatorParent.localPosition, _userInputConfig.VirtualJoystickMaxMagnitude);

            var differenceToCircle = _indicatorParent.position - _circleParent.position;
            var rotation = Quaternion.LookRotation(Vector3.forward, differenceToCircle);

            _indicatorParent.localRotation = rotation;            
        }
    }
}