using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.Cheats;
using _Source.Features.GameRound;
using _Source.Features.Hud;
using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour;
using _Source.Features.NovatarBehaviour.Behaviours;
using _Source.Features.NovatarSpawning;
using _Source.Features.ScreenSize;
using _Source.Features.UiScreens;
using _Source.Features.UserInput;
using _Source.Features.ViewManagement;
using _Source.Util;
using UnityEngine;

namespace _Source.Installation
{
    public class GameSceneInstaller : AbstractSceneInstaller
    {
        [SerializeField] private UnityEngine.Camera _sceneCamera;

        protected override void InstallSceneBindings()
        {
            Container.BindInstance(_sceneCamera);

            Container.BindPrefabFactory<HudView, HudView.Factory>();
            Container.BindPrefabFactory<PauseView, PauseView.Factory>();
            Container.BindPrefabFactory<SurvivalStatsView, SurvivalStatsView.Factory>();
            Container.BindPrefabFactory<RoundEndedView, RoundEndedView.Factory>();
            Container.BindPrefabFactory<VirtualJoystickView, VirtualJoystickView.Factory>();
            Container.BindPrefabFactory<SettingsView, SettingsView.Factory>();

            Container.BindInterfacesAndSelfTo<ViewManagementController>().AsSingleNonLazy();

            Container.BindPrefabFactory<AvatarEntity, AvatarEntity.Factory>();
            Container.BindPrefabFactory<NovatarEntity, NovatarEntity.Factory>();

            Container.BindInterfacesAndSelfTo<AvatarStateModel>().AsSingleNonLazy();
            Container.BindFactory<AvatarEntity, AvatarFacade, AvatarFacade.Factory>();

            Container.BindInterfacesAndSelfTo<VirtualJoystickModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<MovementModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<UserInputController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameRoundModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<ScreenSizeModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<ScreenSizeController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingleNonLazy();
            Container.BindFactory<NovatarEntity, NovatarStateModel, IMovementModel, NovatarFacade, NovatarFacade.Factory>();
            Container.BindFactory<NovatarStateModel, NovatarStateModel.Factory>();
            Container.BindFactory<INovatar, INovatarStateModel, NovatarBehaviourTree, NovatarBehaviourTree.Factory>();
            Container.BindFactory<INovatar, INovatarStateModel, SpawningBehaviour, SpawningBehaviour.Factory>();
            Container.BindFactory<INovatar, INovatarStateModel, TelemetryBehaviour, TelemetryBehaviour.Factory>();
            Container.BindFactory<INovatar, INovatarStateModel, UnacquaintedBehaviour, UnacquaintedBehaviour.Factory>();
            Container.BindFactory<INovatar, INovatarStateModel, NeutralBehaviour, NeutralBehaviour.Factory>();
            Container.BindFactory<INovatar, INovatarStateModel, FriendBehaviour, FriendBehaviour.Factory>();
            Container.BindFactory<INovatar, INovatarStateModel, EnemyBehaviour, EnemyBehaviour.Factory>();

            Container.BindInterfacesAndSelfTo<CheatController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameSceneInitializer>().AsSingleNonLazy();
        }
    }
}
