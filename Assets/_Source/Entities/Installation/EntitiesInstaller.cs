using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors;
using _Source.Util;
using Zenject;

namespace _Source.Entities.Installation
{
    public class EntitiesInstaller : Installer<EntitiesInstaller>
    {
        [Inject] private readonly AvatarStateFactory _avatarStateFactory;

        public override void InstallBindings()
        {
            Container.BindPrefabFactory<AvatarEntity, AvatarEntity.Factory>();
            Container.BindPrefabFactory<NovatarEntity, NovatarEntity.Factory>();

            Container.BindFactory<AvatarEntity, AvatarFacade, AvatarFacade.Factory>().AsSingle();
            Container.BindFactory<NovatarEntity, IActorStateModel, NovatarFacade, NovatarFacade.Factory>().AsSingle();

            // ToDo find better way
            var actorStateModel = _avatarStateFactory.Create();

            Container.BindInterfacesTo<ActorStateModel>()
                .FromInstance(actorStateModel)
                .AsSingle();
        }
    }
}
