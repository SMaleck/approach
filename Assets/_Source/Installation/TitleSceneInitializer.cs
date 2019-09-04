using _Source.Features.TitleMenu;
using _Source.Features.ViewManagement;
using _Source.Installation.Data;
using Zenject;

namespace _Source.Installation
{
    public class TitleSceneInitializer : AbstractSceneInitializer, IInitializable
    {
        [Inject] private ViewPrefabsConfig _viewPrefabsConfig;
        [Inject] private IViewManagementRegistrar _viewManagementRegistrar;

        [Inject] private TitleView.Factory _titleViewFactory;
        [Inject] private SettingsView.Factory _settingsViewFactory;
        [Inject] private HowToPlayView.Factory _howToPlayViewFactory;

        public void Initialize()
        {
            _titleViewFactory
                .Create(_viewPrefabsConfig.TitleViewPrefab)
                .Initialize();

            var settingsView = _settingsViewFactory
                .Create(_viewPrefabsConfig.SettingsViewPrefab);
            settingsView.Initialize();
            
            _viewManagementRegistrar.RegisterView(ViewType.Settings, settingsView);

            var howToPlayView = _howToPlayViewFactory
                .Create(_viewPrefabsConfig.HowToPlayViewPrefab);
            howToPlayView.Initialize();

            _viewManagementRegistrar.RegisterView(ViewType.HowToPlay, howToPlayView);
        }
    }
}
