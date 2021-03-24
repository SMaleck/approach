using _Source.Features.Actors.DataComponents;
using _Source.Features.ScreenSize;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Components
{
    public class ScreenCapMonoComponent : AbstractMonoComponent, ITickableMonoComponent
    {
        [Inject] private readonly ScreenSizeModel _screenSizeModel;
        private TransformDataComponent _transformDataComponent;

        private float xMin;
        private float xMax;

        private float yMin;
        private float yMax;

        protected override void OnSetup()
        {
            _transformDataComponent = Actor.Get<TransformDataComponent>();

            xMin = -_screenSizeModel.WidthExtendUnits;
            xMax = _screenSizeModel.WidthExtendUnits;

            yMin = -_screenSizeModel.HeightExtendUnits;
            yMax = _screenSizeModel.HeightExtendUnits;
        }

        public void Tick()
        {
            var position = _transformDataComponent.Position;
            var clampedX = Mathf.Clamp(position.x, xMin, xMax);
            var clampedY = Mathf.Clamp(position.y, yMin, yMax);

            _transformDataComponent.SetPositionSafe(
                new Vector2(clampedX, clampedY));
        }
    }
}
