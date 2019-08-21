using Zenject;

namespace _Source.Features.SceneManagement
{
    public class SceneStartController : IInitializable
    {
        private readonly SceneManagementController _sceneManagementController;

        public SceneStartController(SceneManagementController sceneManagementController)
        {
            _sceneManagementController = sceneManagementController;
        }

        public void Initialize()
        {
            _sceneManagementController.OnSceneInitializationCompleted();
        }
    }
}
