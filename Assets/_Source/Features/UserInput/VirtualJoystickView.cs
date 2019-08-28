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

        [Header("VJ Circle")]
        [SerializeField] private Transform _circleParent;

        [Header("VJ Indicator")]
        [SerializeField] private Transform _indicatorParent;        

        private IReadOnlyVirtualJoystickModel _virtualJoystickModel;
        private UnityEngine.Camera _sceneCamera;

        [Inject]
        private void Inject(
            IReadOnlyVirtualJoystickModel virtualJoystickModel,
            UnityEngine.Camera sceneCamera)
        {
            _virtualJoystickModel = virtualJoystickModel;
            _sceneCamera = sceneCamera;
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
            SetPositionFromPointer(position, _circleParent);
        }

        private void UpdateIndicatorPosition(Vector2 position)
        {
            SetPositionFromPointer(position, _indicatorParent);
        }

        private void SetPositionFromPointer(Vector2 pointerScreenPosition, Transform target)
        {
            target.position = pointerScreenPosition;
        }
    }
}