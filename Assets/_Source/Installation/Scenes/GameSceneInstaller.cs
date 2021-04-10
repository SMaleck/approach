using _Source.Debug.Installation;
using _Source.Features.ActorBehaviours.Installation;
using _Source.Features.ActorEntities.Installation;
using _Source.Features.Actors;
using _Source.Features.Actors.Installation;
using _Source.Features.AvatarRelationships;
using _Source.Features.FeatureToggles.Installation;
using _Source.Features.GameRound.Installation;
using _Source.Features.Movement;
using _Source.Features.Movement.Installation;
using _Source.Features.PlayerStatistics.Installation;
using _Source.Features.ScreenSize;
using _Source.Features.Tutorials.Installation;
using _Source.Features.UiHud;
using _Source.Features.UiScreens;
using _Source.Features.UserInput;
using _Source.Features.ViewManagement;
using _Source.Util;
using UnityEngine;

namespace _Source.Installation.Scenes
{
    public class GameSceneInstaller : AbstractSceneInstaller
    {
        [SerializeField] private UnityEngine.Camera _sceneCamera;

        protected override void InstallSceneBindings()
        {
            Container.BindInstance(_sceneCamera);

            ActorsInstaller.Install(Container);
            ActorEntitiesInstaller.Install(Container);
            ActorBehavioursInstaller.Install(Container);
            MovementInstaller.Install(Container);
            GameRoundInstaller.Install(Container);
            PlayerStatisticsInstaller.Install(Container);
            TutorialsInstaller.Install(Container);
            FeatureTogglesInstaller.Install(Container);

            Container.BindPrefabFactory<HudView, HudView.Factory>();
            Container.BindPrefabFactory<PauseView, PauseView.Factory>();
            Container.BindPrefabFactory<RoundEndedView, RoundEndedView.Factory>();
            Container.BindPrefabFactory<VirtualJoystickView, VirtualJoystickView.Factory>();
            Container.BindPrefabFactory<SettingsView, SettingsView.Factory>();

            Container.BindInterfacesAndSelfTo<ViewManagementController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<VirtualJoystickModel>().AsSingleNonLazy();
            Container.BindFactory<IActorStateModel, InputMovementController, InputMovementController.Factory>();

            Container.BindInterfacesAndSelfTo<RelationshipStatsModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<RelationshipStatsController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<ScreenSizeModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<ScreenSizeController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameSceneInitializer>().AsSingleNonLazy();
        }
    }
}
