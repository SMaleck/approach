using _Source.Features.Actors.DataComponents;
using _Source.Features.ScreenSize;
using Zenject;

namespace _Source.Features.ActorEntities.Components
{
    public class ScreenCapMonoComponent : AbstractMonoComponent, ITickableMonoComponent
    {
        [Inject] private readonly ScreenSizeModel _screenSizeModel;

        private TransformDataComponent _transformDataComponent;

        protected override void OnSetup()
        {
            _transformDataComponent = Actor.Get<TransformDataComponent>();
        }

        public void Tick()
        {
            var clampedPosition = _screenSizeModel.GetClampedPosition(
                _transformDataComponent.Position);

            _transformDataComponent.SetPositionSafe(clampedPosition);
        }
    }
}
