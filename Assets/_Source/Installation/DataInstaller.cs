using _Source.Entities.ActorEntities.Avatar.Data;
using _Source.Entities.ActorEntities.Novatar.Data;
using Zenject;

namespace _Source.Installation
{
    public class DataInstaller : Installer<DataInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AvatarData>().AsSingle();
            Container.BindInterfacesAndSelfTo<NovatarData>().AsSingle();
        }
    }
}
