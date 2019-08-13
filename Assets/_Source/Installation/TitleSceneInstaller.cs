using _Source.Entities;
using _Source.Features.GameRound;
using _Source.Features.UserInput;
using _Source.Util;
using Zenject;

namespace _Source.Installation
{
    public class TitleSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TitleSceneInitializer>().AsSingleNonLazy();
        }
    }
}
