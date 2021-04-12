using _Source.App;
using _Source.Services.Environment;
using _Source.Util.Debug;
using UnityEngine;
using Zenject;

namespace _Source.Debug.Installation
{
    [CreateAssetMenu(fileName = nameof(ProjectDebugInstaller), menuName = Constants.InstallersMenu + nameof(ProjectDebugInstaller))]
    public class ProjectDebugInstaller : ScriptableObjectInstaller<ProjectDebugInstaller>
    {
        [SerializeField] private FpsProfilerView _fpoFpsProfilerView;

        public override void InstallBindings()
        {
            if (!EnvironmentService.IsDebugStatic)
            {
                return;
            }

            Container.BindInterfacesAndSelfTo<FpsProfiler>().AsSingle();
            Container.Bind<FpsProfilerView>()
                .FromComponentInNewPrefab(_fpoFpsProfilerView)
                .AsSingle()
                .NonLazy();
        }
    }
}
