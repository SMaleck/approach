using Zenject;

namespace _Source.Debug.Installation
{
    public class DebugInstaller : Installer<DebugInstaller>
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
