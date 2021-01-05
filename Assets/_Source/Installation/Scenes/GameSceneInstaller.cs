using _Source.Entities;
using _Source.Entities.ActorEntities.Installation;
using _Source.Entities.Actors;
using _Source.Entities.Actors.Installation;
using _Source.Entities.Installation;
using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Installation;
using _Source.Features.AvatarRelationships;
using _Source.Features.Cheats;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Features.Movement.Data;
using _Source.Features.NovatarBehaviour;
using _Source.Features.NovatarBehaviour.Sensors;
using _Source.Features.ScreenSize;
using _Source.Features.UiHud;
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

            Container.BindFactory<IMovementData, MovementModel, MovementModel.Factory>();
            Container.BindFactory<MovementModel, IMonoEntity, MovementController, MovementController.Factory>();

            Container.BindFactory<IMonoEntity, IMovementModel, MovementComponent, MovementComponent.Factory>();
            Container.BindInterfacesAndSelfTo<VirtualJoystickModel>().AsSingleNonLazy();
            Container.BindFactory<MovementModel, UserInputController, UserInputController.Factory>();

            Container.BindInterfacesAndSelfTo<GameRoundModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<RelationshipStatsModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<RelationshipStatsController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<ScreenSizeModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<ScreenSizeController>().AsSingleNonLazy();

            Container.BindFactory<INovatar, IActorStateModel, ISensorySystem, MovementController, NovatarBehaviourTree, NovatarBehaviourTree.Factory>();

            Container.BindInterfacesAndSelfTo<CheatController>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameSceneInitializer>().AsSingleNonLazy();

            ActorsInstaller.Install(Container);
            ActorEntitiesInstaller.Install(Container);
            ActorBehavioursInstaller.Install(Container);
            EntitiesInstaller.Install(Container);
        }
    }
}
