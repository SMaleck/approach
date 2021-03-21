using _Source.Features.Actors;

namespace _Source.Features.ActorEntities.Avatar
{
    public interface IAvatarLocator
    {
        IActorStateModel AvatarActorStateModel { get; }
    }
}