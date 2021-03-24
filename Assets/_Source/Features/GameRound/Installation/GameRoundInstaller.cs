using _Source.Util;
using Zenject;

namespace _Source.Features.GameRound.Installation
{
    public class GameRoundInstaller : Installer<GameRoundInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameRoundModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<GameRoundController>().AsSingleNonLazy();
        }
    }
}
