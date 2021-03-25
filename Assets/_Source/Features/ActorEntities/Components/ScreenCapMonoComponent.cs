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

        private float _xMin;
        private float _xMax;

        private float _yMin;
        private float _yMax;

        protected override void OnSetup()
        {
            _transformDataComponent = Actor.Get<TransformDataComponent>();

            _xMin = -_screenSizeModel.WidthExtendUnits;
            _xMax = _screenSizeModel.WidthExtendUnits;

            _yMin = -_screenSizeModel.HeightExtendUnits;
            _yMax = _screenSizeModel.HeightExtendUnits;
        }

        public void Tick()
        {
            var position = _transformDataComponent.Position;
            var clampedX = Mathf.Clamp(position.x, _xMin, _xMax);
            var clampedY = Mathf.Clamp(position.y, _yMin, _yMax);

            _transformDataComponent.SetPositionSafe(
                new Vector2(clampedX, clampedY));
        }
    }
}
