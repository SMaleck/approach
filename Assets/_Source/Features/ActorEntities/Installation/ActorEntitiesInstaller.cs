using _Source.Features.ActorEntities.Avatar;
using _Source.Features.ActorEntities.Novatar;
using _Source.Features.Actors;
using _Source.Util;
using Zenject;

namespace _Source.Features.ActorEntities.Installation
{
    public class ActorEntitiesInstaller : Installer<ActorEntitiesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<AvatarEntity, IActorStateModel, AvatarFacade, AvatarFacade.Factory>().AsSingle();
            Container.BindPrefabFactory<AvatarEntity, AvatarEntity.Factory>();
            Container.BindExecutionOrder<AvatarSpawner>(-1);
            Container.BindInterfacesTo<AvatarSpawner>().AsSingle();

            Container.BindFactory<NovatarEntity, IActorStateModel, NovatarFacade, NovatarFacade.Factory>().AsSingle();
            Container.BindPrefabFactory<NovatarEntity, NovatarEntity.Factory>();
            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawningOrchestrator>().AsSingle().NonLazy();
        }
    }
}
