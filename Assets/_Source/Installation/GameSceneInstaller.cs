using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.AvatarState;
using _Source.Features.Cheats;
using _Source.Features.GameRound;
using _Source.Features.GameWorld;
using _Source.Features.NovatarBehaviour;
using _Source.Features.NovatarBehaviour.Behaviours;
using _Source.Features.NovatarSpawning;
using _Source.Features.UserInput;
using _Source.Features.ViewManagement;
using _Source.Util;
using Assets._Source.Features.Movement;
using UnityEngine;

namespace _Source.Installation
{
    public class GameSceneInstaller : AbstractSceneInstaller
    {
        [SerializeField] private UnityEngine.Camera _sceneCamera;

        protected override void InstallSceneBindings()
        {
            Container.BindInstance(_sceneCamera);

            Container.BindInterfacesAndSelfTo<ViewManagementController>().AsSingleNonLazy();

            Container.BindPrefabFactory<AvatarEntity, AvatarEntity.Factory>();
            Container.BindInterfacesAndSelfTo<AvatarStateModel>().AsSingleNonLazy();
            Container.BindFactory<AvatarEntity, AvatarFacade, AvatarFacade.Factory>();

            Container.BindPrefabFactory<SurvivalStatsView, SurvivalStatsView.Factory>();
            Container.BindPrefabFactory<RoundEndedView, RoundEndedView.Factory>();

            Container.BindInterfacesAndSelfTo<VirtualJoystickModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<MovementModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<UserInputController>().AsSingleNonLazy();
            Container.BindPrefabFactory<VirtualJoystickView, VirtualJoystickView.Factory>();
            
            Container.BindInterfacesAndSelfTo<GameRoundModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<ScreenSizeModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<ScreenSizeController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingleNonLazy();
            Container.BindFactory<NovatarEntity, NovatarStateModel, NovatarFacade, NovatarFacade.Factory>();
            Container.BindPrefabFactory<NovatarEntity, NovatarEntity.Factory>();
            Container.BindFactory<NovatarStateModel, NovatarStateModel.Factory>();
            Container.BindFactory<INovatar, NovatarStateModel, NovatarBehaviourTree, NovatarBehaviourTree.Factory>();
            Container.BindFactory<INovatar, NovatarStateModel, SpawningBehaviour, SpawningBehaviour.Factory>();
            Container.BindFactory<INovatar, NovatarStateModel, TelemetryBehaviour, TelemetryBehaviour.Factory>();
            Container.BindFactory<INovatar, NovatarStateModel, UnacquaintedBehaviour, UnacquaintedBehaviour.Factory>();
            Container.BindFactory<INovatar, NovatarStateModel, NeutralBehaviour, NeutralBehaviour.Factory>();
            Container.BindFactory<INovatar, NovatarStateModel, FriendBehaviour, FriendBehaviour.Factory>();
            Container.BindFactory<INovatar, NovatarStateModel, EnemyBehaviour, EnemyBehaviour.Factory>();

            Container.BindInterfacesAndSelfTo<CheatController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameSceneInitializer>().AsSingleNonLazy();
        }
    }
}
