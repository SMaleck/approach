using Zenject;

namespace _Source.Debug.Installation
{
    public class FeatureDebugInstaller : Installer<FeatureDebugInstaller>
    {
        public override void InstallBindings()
        {
            if (!UnityEngine.Debug.isDebugBuild)
            {
                return;
            }

            Container.BindInterfacesTo<SpawnerCheatController>().AsSingle().NonLazy();
        }
    }
}
