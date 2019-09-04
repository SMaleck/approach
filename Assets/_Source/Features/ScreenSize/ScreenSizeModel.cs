using _Source.Util;
using UnityEngine;

namespace _Source.Features.ScreenSize
{
    public class ScreenSizeModel : AbstractDisposable
    {
        private readonly Camera _sceneCamera;

        public float WidthUnits { get; private set; }
        public float HeightUnits { get; private set; }

        public float WidthExtendUnits => WidthUnits / 2;
        public float HeightExtendUnits => HeightUnits / 2;

        public ScreenSizeModel(UnityEngine.Camera sceneCamera)
        {
            _sceneCamera = sceneCamera;

            if (sceneCamera.orthographic)
            {
                CalculateOrthographicBased();
            }
            else
            {
                CalculatePerspectiveBased();
            }
        }

        private void CalculateOrthographicBased()
        {
            HeightUnits = 2f * _sceneCamera.orthographicSize;
            WidthUnits = HeightUnits * _sceneCamera.aspect;
        }

        private void CalculatePerspectiveBased()
        {
            var distance = Mathf.Abs(_sceneCamera.transform.position.z);

            HeightUnits = 2.0f * distance * Mathf.Tan(_sceneCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            WidthUnits = HeightUnits * _sceneCamera.aspect;
        }
    }
}
