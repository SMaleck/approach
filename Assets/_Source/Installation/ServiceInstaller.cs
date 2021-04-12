using _Source.Services.Environment;
using _Source.Services.Random;
using _Source.Services.SavegameSystem.Installation;
using _Source.Util;
using Zenject;

namespace _Source.Installation
{
    public class ServiceInstaller : Installer<ServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<EnvironmentService>().AsSingleNonLazy();
            Container.BindInterfacesTo<RandomNumberService>().AsSingleNonLazy();

            SavegameServiceInstaller.Install(Container);
            SavegameInstaller.Install(Container);
        }
    }
}
