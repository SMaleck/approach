using _Source.Features.TitleMenu;
using _Source.Features.ViewManagement;
using _Source.Util;
using Zenject;

namespace _Source.Installation
{
    public class TitleSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ViewManagementController>().AsSingleNonLazy();

            Container.BindPrefabFactory<TitleView, TitleView.Factory>();
            Container.BindPrefabFactory<SettingsView, SettingsView.Factory>();
            Container.BindPrefabFactory<HowToPlayView, HowToPlayView.Factory>();

            Container.BindInterfacesAndSelfTo<TitleSceneInitializer>().AsSingleNonLazy();
        }
    }
}
