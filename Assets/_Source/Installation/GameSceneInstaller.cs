using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.GameRound;
using _Source.Features.GameWorld;
using _Source.Features.NovatarBehaviour;
using _Source.Features.SurvivalStats;
using _Source.Features.UserInput;
using _Source.Util;
using Zenject;

namespace _Source.Installation
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindPrefabFactory<AvatarEntity, AvatarEntity.Factory>();
            Container.BindPrefabFactory<NovatarEntity, NovatarEntity.Factory>();

            Container.BindPrefabFactory<SurvivalStatsView, SurvivalStatsView.Factory>();
            Container.BindPrefabFactory<RoundEndedView, RoundEndedView.Factory>();

            Container.BindInterfacesAndSelfTo<UserInputModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<UserInputController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<SurvivalStatsModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<SurvivalStatsController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameRoundModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<ScreenSizeModel>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<NovatarSpawner>().AsSingleNonLazy();
            Container.BindFactory<NovatarStateModel, NovatarStateModel.Factory>();
            Container.BindFactory<NovatarEntity, NovatarStateModel, NovatarBehaviourTree, NovatarBehaviourTree.Factory>();

            Container.BindInterfacesAndSelfTo<GameSceneInitializer>().AsSingleNonLazy();
        }
    }
}
