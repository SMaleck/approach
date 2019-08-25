using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.UserInput
{
    public class VirtualJoystickView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, VirtualJoystickView> { }

        [Header("Renderer")]
        [SerializeField] private GameObject _virtualJoystickParent;
        [SerializeField] private LineRenderer _lineRenderer;

        [Header("VJ Circle Settings")]
        [SerializeField] private int _circleSegmentCount;
        [SerializeField] private float _circleRadius;
        [SerializeField] private Color _circleColor;

        [Header("VJ Indicator Settings")]
        [SerializeField] private Color _indicatorColor;

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
                .Subscribe(_ => UpdateJoystickPosition())
                .AddTo(Disposer);

            _virtualJoystickModel.CurrentPointerPosition
                .Subscribe(_ => UpdateIndicatorPosition())
                .AddTo(Disposer);

            SetupLineRenderer();
        }

        private void SetupLineRenderer()
        {
            _lineRenderer.positionCount = _circleSegmentCount;
            _lineRenderer.startColor = _circleColor;
            _lineRenderer.endColor = _circleColor;

            var angleSteps = 360f / _circleSegmentCount;

            for (var i = 0; i < _circleSegmentCount; i++)
            {
                var currentAngle = angleSteps * i + 1;

                var x = Mathf.Sin(Mathf.Deg2Rad * currentAngle) * _circleRadius;
                var y = Mathf.Cos(Mathf.Deg2Rad * currentAngle) * _circleRadius;

                _lineRenderer.SetPosition(i, new Vector3(x, y, transform.position.z));
            }
        }

        private void UpdateJoystickPosition()
        {
            var pointerPosition = _virtualJoystickModel.StartPointerPosition.Value;
            var sceneCameraZDistance = Mathf.Abs(_sceneCamera.transform.position.z);

            var targetPosition = _sceneCamera.ScreenToWorldPoint(
                new Vector3(pointerPosition.x, pointerPosition.y, sceneCameraZDistance));

            _virtualJoystickParent.transform.position = targetPosition;
        }

        private void UpdateIndicatorPosition()
        {
        }
    }
}