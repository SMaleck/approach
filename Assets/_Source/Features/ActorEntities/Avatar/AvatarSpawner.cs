using _Source.Entities.Avatar;
using _Source.Features.ActorEntities.Avatar.Config;
using _Source.Features.Actors;
using _Source.Features.Movement;
using _Source.Features.UserInput;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.ActorEntities.Avatar
{
    public class AvatarSpawner : AbstractDisposable, IInitializable, IAvatarLocator
    {
        [Inject] private readonly AvatarStateFactory _avatarStateFactory;
        [Inject] private AvatarEntity.Factory _avatarEntityFactory;
        [Inject] private AvatarFacade.Factory _avatarFacadeFactory;
        [Inject] private AvatarConfig _avatarConfig;
        [Inject] private MovementModel.Factory _movementModelFactory;
        [Inject] private MovementComponent.Factory _movementComponentFactory;
        [Inject] private UserInputController.Factory _userInputControllerFactory;

        // ToDo V0 This can probably be exposed cleaner
        public IActorStateModel AvatarActorStateModel { get; private set; }

        public void Initialize()
        {
            AvatarActorStateModel = _avatarStateFactory.Create();

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
