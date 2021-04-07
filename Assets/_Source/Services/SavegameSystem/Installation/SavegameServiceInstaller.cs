using _Source.Services.SavegameSystem.Storage;
using Packages.SavegameSystem;
using Zenject;

namespace _Source.Services.SavegameSystem.Installation
{
    public class SavegameServiceInstaller : Installer<SavegameServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SavegameLocalStorage>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SavegameService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SavegameFactory>().AsSingle();
        }
    }
}
