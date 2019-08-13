using _Source.Entities;
using _Source.Features.GameRound;
using _Source.Features.SceneManagement;
using _Source.Installation.Data;
using Zenject;

namespace _Source.Installation
{
    public class TitleSceneInitializer : IInitializable
    {
        [Inject] private DiContainer _container;
        [Inject] private ViewPrefabsConfig _viewPrefabsConfig;

        [Inject] private SceneManagementController _sceneManagementController;

        public void Initialize()
        {
            _sceneManagementController.ToGame();
        }
    }
}
