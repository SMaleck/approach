using _Source.Features.UiScreens;
using _Source.Features.ViewManagement;
using _Source.Util;

namespace _Source.Installation
{
    public class TitleSceneInstaller : AbstractSceneInstaller
    {
        protected override void InstallSceneBindings()
        {
            Container.BindInterfacesAndSelfTo<ViewManagementController>().AsSingleNonLazy();

            Container.BindPrefabFactory<TitleView, TitleView.Factory>();
            Container.BindPrefabFactory<SettingsView, SettingsView.Factory>();
            Container.BindPrefabFactory<HowToPlayView, HowToPlayView.Factory>();

            Container.BindInterfacesAndSelfTo<TitleSceneInitializer>().AsSingleNonLazy();
        }
    }
}
