using _Source.Features.ActorBehaviours.Creation;
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
            Container.BindPrefabFactory<MonoEntity, MonoEntity.Factory>();

            Container.BindFactory<IMonoEntity, IActorStateModel, AvatarFacade, AvatarFacade.Factory>().AsSingle();
            Container.BindExecutionOrder<AvatarSpawner>(-1);
            Container.BindInterfacesTo<AvatarSpawner>().AsSingle();

            Container.BindFactory<IMonoEntity, IActorStateModel, BehaviourTree, NovatarFacade, NovatarFacade.Factory>().AsSingle();
            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawningOrchestrator>().AsSingle().NonLazy();
        }
    }
}
