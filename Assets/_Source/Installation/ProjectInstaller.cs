﻿using _Source.Features.SceneManagement;
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

            Container.BindInterfacesAndSelfTo<ProjectInitializer>().AsSingleNonLazy();

            DataInstaller.Install(Container);
        }
    }
}
