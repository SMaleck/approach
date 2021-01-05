using _Source.Entities.ActorEntities.Avatar;
using _Source.Entities.ActorEntities.Novatar;
using Zenject;

namespace _Source.Entities.ActorEntities.Installation
{
    public class ActorEntitiesInstaller : Installer<ActorEntitiesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AvatarSpawner>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingle().NonLazy();
        }
    }
}
