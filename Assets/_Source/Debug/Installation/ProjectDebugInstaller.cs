using _Source.Util.Debug;
using Zenject;

namespace _Source.Debug.Installation
{
    public class ProjectDebugInstaller : Installer<ProjectDebugInstaller>
    {
        public override void InstallBindings()
        {
            if (!UnityEngine.Debug.isDebugBuild)
            {
                return;
            }

            Container.BindInterfacesAndSelfTo<FpsProfiler>().AsSingle();
        }
    }
}
