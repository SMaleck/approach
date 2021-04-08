using _Source.Features.Tutorials.Controllers;
using _Source.Features.Tutorials.Savegame;
using _Source.Features.Tutorials.Views;
using _Source.Util;
using Zenject;

namespace _Source.Features.Tutorials.Installation
{
    public class TutorialsInstaller : Installer<TutorialsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TutorialsCollectionModel>().AsSingle();
            Container.BindFactory<ITutorialSavegame, TutorialModel, TutorialModel.Factory>().AsSingle();

            Container.BindInterfacesTo<ControlsTutorialController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<LifeTutorialController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<NovatarsTutorialController>().AsSingle().NonLazy();

            Container.BindPrefabFactory<TutorialView, TutorialView.Factory>();

            Container.BindInterfacesTo<TutorialsInitializer>().AsSingle();
        }
    }
}
