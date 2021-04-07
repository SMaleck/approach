using Zenject;

namespace _Source.Features.PlayerStatistics.Installation
{
    public class PlayerStatisticsInstaller : Installer<PlayerStatisticsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameRoundStatisticsModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameRoundStatisticsController>().AsSingle().NonLazy();

            Container.BindInterfacesTo<PlayerStatisticsModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerStatisticsController>().AsSingle().NonLazy();
        }
    }
}
