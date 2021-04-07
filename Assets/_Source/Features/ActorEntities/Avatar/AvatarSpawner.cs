using _Source.Features.ActorEntities.Config;
using _Source.Features.Actors;
using _Source.Features.Actors.Creation;
using _Source.Features.Movement;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.ActorEntities.Avatar
{
    public class AvatarSpawner : AbstractDisposable, IInitializable, IAvatarLocator
    {
        [Inject] private readonly IAvatarActorFactory _avatarActorFactory;
        [Inject] private readonly MonoEntity.Factory _entityFactory;
        [Inject] private readonly AvatarFacade.Factory _avatarFacadeFactory;
        [Inject] private readonly ActorEntitiesConfig _actorEntitiesConfig;
        [Inject] private readonly InputMovementController.Factory _userInputControllerFactory;

        // ToDo V2 This can probably be exposed cleaner
        public IActorStateModel AvatarActor { get; private set; }
        
        private readonly IReactiveProperty<bool> _isAvatarSpawned;
        public IReadOnlyReactiveProperty<bool> IsAvatarSpawned => _isAvatarSpawned;

        public AvatarSpawner()
        {
            _isAvatarSpawned = new ReactiveProperty<bool>().AddTo(Disposer);
        }

        public void Initialize()
        {
            AvatarActor = _avatarActorFactory.CreateAvatar();

            var avatarEntity = _entityFactory.Create(
                _actorEntitiesConfig.AvatarPrefab);

            _avatarFacadeFactory.Create(
                avatarEntity,
                AvatarActor);

            _userInputControllerFactory
                .Create(AvatarActor);

            _isAvatarSpawned.Value = true;
        }
    }
}
