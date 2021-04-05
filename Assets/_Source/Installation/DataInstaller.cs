using _Source.Features.ActorEntities.Avatar.Data;
using _Source.Features.ActorEntities.Novatar.Data;
using _Source.Features.Movement.Data;
using Zenject;

namespace _Source.Installation
{
    public class DataInstaller : Installer<DataInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AvatarData>().AsSingle();
            Container.BindInterfacesAndSelfTo<NovatarData>().AsSingle();
            Container.BindInterfacesAndSelfTo<MovementDataRepository>().AsSingle();
        }
    }
}
