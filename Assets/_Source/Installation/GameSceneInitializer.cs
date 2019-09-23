using _Source.Entities.Avatar;
using _Source.Features.Movement;
using _Source.Features.UiHud;
using _Source.Features.UiScreens;
using _Source.Features.UserInput;
using _Source.Features.ViewManagement;
using _Source.Installation.Data;
using UniRx;
using Zenject;

namespace _Source.Installation
{
    public class GameSceneInitializer : AbstractSceneInitializer, IInitializable
    {
        [Inject] private ViewPrefabsConfig _viewPrefabsConfig;
        [Inject] private IViewManagementRegistrar _viewManagementRegistrar;

        [Inject] private AvatarEntity.Factory _avatarFactory;
        [Inject] private AvatarFacade.Factory _avatarFacadeFactory;
        [Inject] private AvatarConfig _avatarConfig;
        [Inject] private MovementModel.Factory _movementModelFactory;
        [Inject] private MovementComponent.Factory _movementComponentFactory;
        [Inject] private UserInputController.Factory _userInputControllerFactory;        

        [Inject] private HudView.Factory _hudViewFactory;
        [Inject] private PauseView.Factory _pauseViewFactory;
        [Inject] private SettingsView.Factory _settingsViewFactory;
        [Inject] private SurvivalStatsView.Factory _survivalStatsViewFactory;
        [Inject] private RoundEndedView.Factory _roundEndedViewFactory;
        [Inject] private VirtualJoystickView.Factory _virtualJoystickViewFactory;

        public void Initialize()
        {
            var avatar = _avatarFactory.Create(_avatarConfig.AvatarPrefab);

            var avatarFacade = _avatarFacadeFactory.Create(avatar);
            avatarFacade.AddTo(SceneDisposer);

            var movementModel = _movementModelFactory
                .Create(_avatarConfig.MovementConfig)
                .AddTo(SceneDisposer);

            _userInputControllerFactory
                .Create(movementModel)
                .AddTo(SceneDisposer);

            _movementComponentFactory
                .Create(avatarFacade, movementModel)
                .AddTo(SceneDisposer);

            SceneContainer.BindInterfacesAndSelfTo<AvatarFacade>()
                .FromInstance(avatarFacade);

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

            _survivalStatsViewFactory
                .Create(_viewPrefabsConfig.SurvivalStatsViewPrefab)
                .Initialize();

            var roundEndView = _roundEndedViewFactory
                .Create(_viewPrefabsConfig.RoundEndedViewPrefab);
            roundEndView.Initialize();
            _viewManagementRegistrar.RegisterView(ViewType.RoundEnd, roundEndView);

            _virtualJoystickViewFactory
                .Create(_viewPrefabsConfig.VirtualJoystickViewPrefab)
                .Initialize();
        }
    }
}
