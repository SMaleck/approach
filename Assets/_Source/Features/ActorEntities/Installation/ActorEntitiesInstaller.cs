using _Source.Features.ActorEntities.Avatar;
using _Source.Features.ActorEntities.Novatar;
using Zenject;

namespace _Source.Features.ActorEntities.Installation
{
    public class ActorEntitiesInstaller : Installer<ActorEntitiesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AvatarStateFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NovatarStateFactory>().AsSingle().NonLazy();
        }
    }
}
