using _Source.Features.ActorEntities.Novatar;
using Zenject;

namespace _Source.Features.ActorEntities.Installation
{
    public class NovatarSpawningInstaller : Installer<NovatarSpawningInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawningOrchestrator>().AsSingle().NonLazy();
        }
    }
}
