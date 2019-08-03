using _Source.Entities;
using _Source.Features.GameRound;
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
            Container.BindPrefabFactory<Avatar, Avatar.Factory>();
            Container.BindPrefabFactory<Novatar, Novatar.Factory>();

            Container.BindPrefabFactory<SurvivalStatsView, SurvivalStatsView.Factory>();
            Container.BindPrefabFactory<RoundEndedView, RoundEndedView.Factory>();

            Container.BindInterfacesAndSelfTo<UserInputModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<UserInputController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<SurvivalStatsModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<SurvivalStatsController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameRoundModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameInitializer>().AsSingleNonLazy();
        }
    }
}
