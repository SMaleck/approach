using _Source.Features.SceneManagement;
using Zenject;

namespace _Source.Installation
{
    public abstract class AbstractSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallSceneBindings();
            PostInstall();
        }

        protected abstract void InstallSceneBindings();

        private void PostInstall()
        {
            Container.BindExecutionOrder<SceneStartController>(999);
            Container.BindInterfacesAndSelfTo<SceneStartController>().AsSingle().NonLazy();
        }
    }
}
