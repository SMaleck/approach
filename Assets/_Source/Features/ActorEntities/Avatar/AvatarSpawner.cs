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
        [Inject] private readonly AvatarEntity.Factory _avatarEntityFactory;
        [Inject] private readonly AvatarFacade.Factory _avatarFacadeFactory;
        [Inject] private readonly AvatarConfig _avatarConfig;
        [Inject] private readonly MovementModel.Factory _movementModelFactory;
        [Inject] private readonly MovementComponent.Factory _movementComponentFactory;
        [Inject] private readonly UserInputController.Factory _userInputControllerFactory;

        // ToDo V0 This can probably be exposed cleaner
        public IActorStateModel AvatarActorStateModel { get; private set; }

        public void Initialize()
        {
            AvatarActorStateModel = _avatarActorFactory.CreateAvatar();

            var avatarEntity = _avatarEntityFactory
                .Create(_avatarConfig.AvatarPrefab);

            var avatarFacade = _avatarFacadeFactory
                .Create(avatarEntity, AvatarActorStateModel);

            var movementModel = _movementModelFactory
                .Create(AvatarActorStateModel)
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
