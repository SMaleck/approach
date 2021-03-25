using _Source.Features.Actors;
using Zenject;

namespace _Source.Features.ActorEntities.Avatar
{
    // ToDo V2 Can this class be removed completely?
    public class AvatarFacade : EntityFacade
    {
        public new class Factory : PlaceholderFactory<IMonoEntity, IActorStateModel, AvatarFacade> { }

        public AvatarFacade(
            IMonoEntity entity,
            IActorStateModel actor)
            : base(entity, actor)
        {
        }
    }
}
