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
    public class AvatarSpawner : AbstractDisposable, IInitializable
    {
        private readonly DiContainer _sceneContainer;
        private readonly IActorStateModel _avatarActorStateModel;

        [Inject] private AvatarEntity.Factory _avatarFactory;
        [Inject] private AvatarFacade.Factory _avatarFacadeFactory;
        [Inject] private AvatarConfig _avatarConfig;
        [Inject] private MovementModel.Factory _movementModelFactory;
        [Inject] private MovementComponent.Factory _movementComponentFactory;
        [Inject] private UserInputController.Factory _userInputControllerFactory;

        // ToDo V0 Do not inject, use service locator pattern instead
        public AvatarSpawner(
            [InjectLocal] DiContainer sceneContainer,
            IActorStateModel avatarActorStateModel)
        {
            _sceneContainer = sceneContainer;
            _avatarActorStateModel = avatarActorStateModel;
        }

        public void Initialize()
        {
            var avatar = _avatarFactory.Create(_avatarConfig.AvatarPrefab);

            var avatarFacade = _avatarFacadeFactory
                .Create(avatar);

            var movementModel = _movementModelFactory
                .Create(_avatarActorStateModel)
                .AddTo(Disposer);

            _userInputControllerFactory
                .Create(movementModel)
                .AddTo(Disposer);

            _movementComponentFactory
                .Create(avatarFacade, movementModel)
                .AddTo(Disposer);

            _sceneContainer.BindInterfacesAndSelfTo<AvatarFacade>()
                .FromInstance(avatarFacade);
        }
    }
}
