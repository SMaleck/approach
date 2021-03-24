using Zenject;

namespace _Source.Features.PlayerStatistics.Installation
{
    public class PlayerStatisticsInstaller : Installer<PlayerStatisticsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerStatisticsModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerStatisticsController>().AsSingle().NonLazy();
        }
    }
}
