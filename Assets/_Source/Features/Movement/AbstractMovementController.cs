using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Util;

namespace _Source.Features.Movement
{
    public class AbstractMovementController : AbstractDisposableFeature
    {
        protected readonly MovementDataComponent MovementDataComponent;
        protected readonly TransformDataComponent TransformDataComponent;

        public AbstractMovementController(IActorStateModel actorStateModel)
        {
            MovementDataComponent = actorStateModel.Get<MovementDataComponent>();
            TransformDataComponent = actorStateModel.Get<TransformDataComponent>();
        }
    }
}
