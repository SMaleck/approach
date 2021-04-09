using _Source.Features.SceneManagement;
using _Source.Util;
using Packages.Logging;
using UnityEngine;
using Zenject;

namespace _Source.Installation
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Application.targetFrameRate = 60;

            Container.BindPrefabFactory<LoadingScreenView, LoadingScreenView.Factory>();
            Container.BindInterfacesAndSelfTo<LoadingScreenModel>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<SceneManagementModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<SceneManagementController>().AsSingleNonLazy();

            Container.BindInterfacesTo<InstanceLogger>().AsSingle();

            // ---------------------------------- INSTALLERS
            DataRepositoryInstaller.Install(Container);
            ServiceInstaller.Install(Container);

            // ---------------------------------- INIT
            Container.BindExecutionOrder<ProjectInitializer>(998);
            Container.BindInterfacesAndSelfTo<ProjectInitializer>().AsSingleNonLazy();
        }
    }
}
