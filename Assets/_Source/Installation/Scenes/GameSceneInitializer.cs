using _Source.Features.GameRound;
using _Source.Features.Tutorials.Views;
using _Source.Features.UiHud;
using _Source.Features.UiScreens;
using _Source.Features.UserInput;
using _Source.Features.ViewManagement;
using _Source.Installation.Data;
using Zenject;

namespace _Source.Installation.Scenes
{
    public class GameSceneInitializer : AbstractSceneInitializer, IInitializable
    {
        [Inject] private ViewPrefabsConfig _viewPrefabsConfig;
        [Inject] private IViewManagementRegistrar _viewManagementRegistrar;
        [Inject] private GameRoundStateController _gameRoundStateController;

        [Inject] private HudView.Factory _hudViewFactory;
        [Inject] private PauseView.Factory _pauseViewFactory;
        [Inject] private SettingsView.Factory _settingsViewFactory;
        [Inject] private RoundEndedView.Factory _roundEndedViewFactory;
        [Inject] private VirtualJoystickView.Factory _virtualJoystickViewFactory;
        [Inject] private TutorialView.Factory _tutorialViewFactory;

        public void Initialize()
        {
            _hudViewFactory
                .Create(_viewPrefabsConfig.HudViewPrefab)
                .Initialize();

            var pauseView = _pauseViewFactory
                .Create(_viewPrefabsConfig.PauseViewPrefab);
            pauseView.Initialize();
            _viewManagementRegistrar.RegisterView(ViewType.Pause, pauseView);

            var settingsView = _settingsViewFactory
                .Create(_viewPrefabsConfig.SettingsViewPrefab);
            settingsView.Initialize();
            _viewManagementRegistrar.RegisterView(ViewType.Settings, settingsView);

            var roundEndView = _roundEndedViewFactory
                .Create(_viewPrefabsConfig.RoundEndedViewPrefab);
            roundEndView.Initialize();
            _viewManagementRegistrar.RegisterView(ViewType.RoundEnd, roundEndView);

            _virtualJoystickViewFactory
                .Create(_viewPrefabsConfig.VirtualJoystickViewPrefab)
                .Initialize();

            _gameRoundStateController.StartRound();
        }
    }
}
