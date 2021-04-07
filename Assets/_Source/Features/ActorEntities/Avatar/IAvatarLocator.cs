using _Source.Features.Actors;
using UniRx;

namespace _Source.Features.ActorEntities.Avatar
{
    public interface IAvatarLocator
    {
        IActorStateModel AvatarActor { get; }
        IReadOnlyReactiveProperty<bool> IsAvatarSpawned { get; }
    }
}