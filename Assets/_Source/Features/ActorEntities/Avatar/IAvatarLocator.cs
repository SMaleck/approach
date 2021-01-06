using _Source.Entities;
using _Source.Entities.Avatar;
using _Source.Features.Actors;

namespace _Source.Features.ActorEntities.Avatar
{
    public interface IAvatarLocator
    {
        IActorStateModel AvatarActorStateModel { get; }
        IMonoEntity AvatarMonoEntity { get; }
        IDamageReceiver AvatarDamageReceiver { get; }
    }
}