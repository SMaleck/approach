using _Source.Features.ActorEntities.Avatar.Config;
using _Source.Features.Actors;
using _Source.Features.Actors.Creation;
using _Source.Features.Movement;
using _Source.Features.UserInput;
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
        [Inject] private readonly AvatarConfig _avatarConfig;
        [Inject] private readonly MovementModel.Factory _movementModelFactory;
        [Inject] private readonly MovementComponent.Factory _movementComponentFactory;
        [Inject] private readonly UserInputController.Factory _userInputControllerFactory;

        // ToDo V2 This can probably be exposed cleaner
        public IActorStateModel AvatarActor { get; private set; }

        public void Initialize()
        {
            AvatarActor = _avatarActorFactory.CreateAvatar();

            var avatarEntity = _entityFactory
                .Create(_avatarConfig.AvatarPrefab);

            var avatarFacade = _avatarFacadeFactory
                .Create(avatarEntity, AvatarActor);

            var movementModel = _movementModelFactory
                .Create(AvatarActor)
                .AddTo(Disposer);

            _userInputControllerFactory
                .Create(movementModel)
                .AddTo(Disposer);

            _movementComponentFactory
                .Create(avatarFacade, movementModel)
                .AddTo(Disposer);
        }
    }
}
