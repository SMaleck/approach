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
        [SerializeField] private LineRenderer _circleLineRenderer;
        [SerializeField] private int _circleSegmentCount;
        [SerializeField] private float _circleRadius;
        [SerializeField] private Color _circleColor;

        [Header("VJ Indicator")]
        [SerializeField] private Transform _indicatorParent;
        [SerializeField] private LineRenderer _indicatorLineRenderer;
        [SerializeField] private int _indicatorSegmentCount;
        [SerializeField] private float _indicatorRadius;
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
                .Subscribe(UpdateCirclePosition)
                .AddTo(Disposer);

            _virtualJoystickModel.CurrentPointerPosition
                .Subscribe(UpdateIndicatorPosition)
                .AddTo(Disposer);

            SetupCircleRenderer();
            SetupIndicatorRenderer();
        }

        private void SetupCircleRenderer()
        {
            SetupLineRenderer(
                _circleLineRenderer,
                _circleSegmentCount,
                _circleRadius,
                _circleColor);
        }

        private void SetupIndicatorRenderer()
        {
            SetupLineRenderer(
                _indicatorLineRenderer,
                _indicatorSegmentCount,
                _indicatorRadius,
                _indicatorColor);
        }

        private void SetupLineRenderer(
            LineRenderer renderer,
            int segmentCount,
            float radius,
            Color color)
        {
            renderer.positionCount = segmentCount;
            renderer.startColor = color;
            renderer.endColor = color;

            var angleSteps = 360f / segmentCount;

            for (var i = 0; i < segmentCount; i++)
            {
                var currentAngle = angleSteps * i + 1;

                var x = Mathf.Sin(Mathf.Deg2Rad * currentAngle) * radius;
                var y = Mathf.Cos(Mathf.Deg2Rad * currentAngle) * radius;

                renderer.SetPosition(i, new Vector3(x, y, transform.position.z));
            }
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
            var sceneCameraZDistance = Mathf.Abs(_sceneCamera.transform.position.z);

            var targetPosition = _sceneCamera.ScreenToWorldPoint(
                new Vector3(pointerScreenPosition.x, pointerScreenPosition.y, sceneCameraZDistance));

            target.position = targetPosition;
        }
    }
}