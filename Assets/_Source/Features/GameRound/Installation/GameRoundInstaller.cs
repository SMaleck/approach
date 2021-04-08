using _Source.Features.GameRound.Duration;
using _Source.Util;
using Zenject;

namespace _Source.Features.GameRound.Installation
{
    public class GameRoundInstaller : Installer<GameRoundInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameRoundStateModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundStateController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameRoundDurationModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundDurationController>().AsSingleNonLazy();
            Container.BindPrefabFactory<GameRoundDurationView, GameRoundDurationView.Factory>();

            Container.BindInterfacesAndSelfTo<GameRoundEndController>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<GameRoundInitializer>().AsSingleNonLazy();
        }
    }
}
