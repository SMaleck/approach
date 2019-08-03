using _Source.Util;
using UnityEngine.SceneManagement;

namespace _Source.Features.SceneManagement
{
    public class SceneManagementController : AbstractDisposable
    {
        private readonly SceneManagementModel _sceneManagementModel;

        public SceneManagementController(SceneManagementModel sceneManagementModel)
        {
            _sceneManagementModel = sceneManagementModel;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void LoadScene(Scenes sceneToLoad)
        {
            var sceneName = sceneToLoad.ToString();
            SceneManager.LoadSceneAsync(sceneName);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _sceneManagementModel.SetIsLoadingScreenVisible(false);
        }

        public void ToTitle()
        {
            _sceneManagementModel.SetIsLoadingScreenVisible(true);
            LoadScene(Scenes.TitleScene);
        }

        public void ToGame()
        {
            _sceneManagementModel.SetIsLoadingScreenVisible(true);
            LoadScene(Scenes.GameScene);
        }
    }
}
