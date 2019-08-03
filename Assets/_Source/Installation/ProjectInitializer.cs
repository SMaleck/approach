using _Source.Features.SceneManagement;
using _Source.Installation.Data;
using Zenject;

namespace _Source.Installation
{
    public class ProjectInitializer : IInitializable
    {
        [Inject] private DiContainer _container;

        [Inject] private SceneManagementController _sceneManagementController;

        [Inject] private ViewPrefabsConfig _viewPrefabsConfig;
        [Inject] private LoadingScreenView.Factory _loadingScreenViewFactory;

        public void Initialize()
        {
            var loadingScreen = _loadingScreenViewFactory.Create(
                _viewPrefabsConfig.LoadingScreenViewPrefab);

            loadingScreen.Initialize();
            _container.BindInstance(loadingScreen);

            _sceneManagementController.ToTitle();
        }
    }
}
