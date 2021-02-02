using _Source.Debug.Installation;
using _Source.Entities.Installation;
using _Source.Features.ActorBehaviours.Installation;
using _Source.Features.ActorEntities.Installation;
using _Source.Features.Actors.Installation;
using _Source.Features.ActorSensors.Installation;
using _Source.Features.AvatarRelationships;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Features.Movement.Installation;
using _Source.Features.ScreenSize;
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
            ActorSensorsInstaller.Install(Container);
            ActorBehavioursInstaller.Install(Container);
            EntitiesInstaller.Install(Container);
            SpawningInstaller.Install(Container);
            MovementInstaller.Install(Container);

            Container.BindPrefabFactory<HudView, HudView.Factory>();
            Container.BindPrefabFactory<PauseView, PauseView.Factory>();
            Container.BindPrefabFactory<SurvivalStatsView, SurvivalStatsView.Factory>();
            Container.BindPrefabFactory<RoundEndedView, RoundEndedView.Factory>();
            Container.BindPrefabFactory<VirtualJoystickView, VirtualJoystickView.Factory>();
            Container.BindPrefabFactory<SettingsView, SettingsView.Factory>();

            Container.BindInterfacesAndSelfTo<ViewManagementController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<VirtualJoystickModel>().AsSingleNonLazy();
            Container.BindFactory<MovementModel, UserInputController, UserInputController.Factory>();

            Container.BindInterfacesAndSelfTo<GameRoundModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<RelationshipStatsModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<RelationshipStatsController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<ScreenSizeModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<ScreenSizeController>().AsSingleNonLazy();

            DebugInstaller.Install(Container);

            Container.BindInterfacesAndSelfTo<GameSceneInitializer>().AsSingleNonLazy();
        }
    }
}
