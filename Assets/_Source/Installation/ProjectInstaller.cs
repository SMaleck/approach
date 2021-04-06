﻿using _Source.Debug.Installation;
using _Source.Features.SceneManagement;
using _Source.Services.Random;
using _Source.Util;
using Zenject;

namespace _Source.Installation
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindPrefabFactory<LoadingScreenView, LoadingScreenView.Factory>();
            Container.BindInterfacesAndSelfTo<LoadingScreenModel>().AsSingleNonLazy();

            Container.BindInterfacesAndSelfTo<SceneManagementModel>().AsSingleNonLazy();
            Container.BindInterfacesAndSelfTo<SceneManagementController>().AsSingleNonLazy();

            // ---------------------------------- INSTALLERS
            DataRepositoryInstaller.Install(Container);
            ProjectDebugInstaller.Install(Container);

            // ---------------------------------- SERVICES
            Container.BindInterfacesTo<RandomNumberService>().AsSingleNonLazy();

            // ---------------------------------- INIT
            Container.BindInterfacesAndSelfTo<ProjectInitializer>().AsSingleNonLazy();
        }
    }
}
