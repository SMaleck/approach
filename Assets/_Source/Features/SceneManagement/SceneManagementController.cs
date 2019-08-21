using _Source.Util;
using UniRx;
using UnityEngine.SceneManagement;

namespace _Source.Features.SceneManagement
{
    public class SceneManagementController : AbstractDisposable, ISceneManagementController
    {
        private readonly SceneManagementModel _sceneManagementModel;
        private readonly SerialDisposable _loadingScreenVisibilityDisposer;


        public SceneManagementController(SceneManagementModel sceneManagementModel)
        {
            _sceneManagementModel = sceneManagementModel;
            _loadingScreenVisibilityDisposer = new SerialDisposable().AddTo(Disposer);
        }

        private void LoadScene(Scenes sceneToLoad)
        {
            _loadingScreenVisibilityDisposer.Disposable?.Dispose();

            var sceneName = sceneToLoad.ToString();
            SceneManager.LoadSceneAsync(sceneName);
        }

        public void OnSceneInitializationCompleted()
        {
            _sceneManagementModel.SetIsLoadingScreenVisible(false);
            _sceneManagementModel.PublishOnSceneStarted();
        }

        public void ToTitle()
        {
            _sceneManagementModel.SetIsLoadingScreenVisible(true);

            _loadingScreenVisibilityDisposer.Disposable = _sceneManagementModel.OnOpenLoadingScreenCompleted
                .Subscribe(_ => LoadScene(Scenes.TitleScene));
        }

        public void ToGame()
        {
            _sceneManagementModel.SetIsLoadingScreenVisible(true);

            _loadingScreenVisibilityDisposer.Disposable = _sceneManagementModel.OnOpenLoadingScreenCompleted
                .Subscribe(_ => LoadScene(Scenes.GameScene));
        }
    }
}
