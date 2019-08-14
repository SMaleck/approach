using _Source.Features.TitleMenu;
using _Source.Util;
using Zenject;

namespace _Source.Installation
{
    public class TitleSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindPrefabFactory<TitleView, TitleView.Factory>();

            Container.BindInterfacesAndSelfTo<TitleSceneInitializer>().AsSingleNonLazy();
        }
    }
}
