using _Source.Features.Tutorials.Savegame;
using _Source.Features.Tutorials.Controllers;
using Zenject;

namespace _Source.Features.Tutorials.Installation
{
    public class TutorialsInstaller : Installer<TutorialsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TutorialsCollectionModel>().AsSingle();
            Container.BindFactory<TutorialSavegame, TutorialModel, TutorialModel.Factory>().AsSingle();

            Container.BindInterfacesTo<ControlsTutorialController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<LifeTutorialController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<NovatarsTutorialController>().AsSingle().NonLazy();
        }
    }
}
