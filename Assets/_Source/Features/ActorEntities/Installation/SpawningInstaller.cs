using _Source.Features.ActorEntities.Avatar;
using _Source.Features.ActorEntities.Novatar;
using Zenject;

namespace _Source.Features.ActorEntities.Installation
{
    public class SpawningInstaller : Installer<SpawningInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindExecutionOrder<AvatarSpawner>(-1);
            Container.BindInterfacesTo<AvatarSpawner>().AsSingle();

            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawningOrchestrator>().AsSingle().NonLazy();
        }
    }
}
