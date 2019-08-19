using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.AvatarState;
using _Source.Features.Cheats;
using _Source.Features.GameRound;
using _Source.Features.GameWorld;
using _Source.Features.NovatarBehaviour;
using _Source.Features.NovatarBehaviour.SubTrees;
using _Source.Features.NovatarSpawning;
using _Source.Features.UserInput;
using _Source.Features.ViewManagement;
using _Source.Util;
using UnityEngine;
using Zenject;

namespace _Source.Installation
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private UnityEngine.Camera _sceneCamera;

        public override void InstallBindings()
        {
            Container.BindInstance(_sceneCamera);

            Container.BindInterfacesAndSelfTo<ViewManagementController>().AsSingleNonLazy();

            Container.BindPrefabFactory<AvatarEntity, AvatarEntity.Factory>();
            Container.BindPrefabFactory<NovatarEntity, NovatarEntity.Factory>();

            Container.BindPrefabFactory<SurvivalStatsView, SurvivalStatsView.Factory>();
            Container.BindPrefabFactory<RoundEndedView, RoundEndedView.Factory>();

            Container.BindInterfacesAndSelfTo<UserInputModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<UserInputController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<AvatarStateModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<SurvivalStatsController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameRoundModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<ScreenSizeModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<ScreenSizeController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingleNonLazy();
            Container.BindFactory<NovatarEntity, NovatarStateModel, NovatarPoolItem, NovatarPoolItem.Factory>();
            Container.BindFactory<NovatarStateModel, NovatarStateModel.Factory>();
            Container.BindFactory<NovatarEntity, NovatarStateModel, NovatarBehaviourTree, NovatarBehaviourTree.Factory>();
            Container.BindFactory<NovatarEntity, NovatarStateModel, TelemetryBehaviour, TelemetryBehaviour.Factory>();
            Container.BindFactory<NovatarEntity, NovatarStateModel, UnacquaintedBehaviour, UnacquaintedBehaviour.Factory>();
            Container.BindFactory<NovatarEntity, NovatarStateModel, NeutralBehaviour, NeutralBehaviour.Factory>();
            Container.BindFactory<NovatarEntity, NovatarStateModel, FriendBehaviour, FriendBehaviour.Factory>();
            Container.BindFactory<NovatarEntity, NovatarStateModel, EnemyBehaviour, EnemyBehaviour.Factory>();

            Container.BindInterfacesAndSelfTo<CheatController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameSceneInitializer>().AsSingleNonLazy();
        }
    }
}
