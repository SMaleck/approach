using _Source.Util;
using System;
using UniRx;
using UnityEngine.SceneManagement;

namespace _Source.Features.SceneManagement
{
    public class SceneManagementController : AbstractDisposable, ISceneManagementController
    {
        private readonly SceneManagementModel _sceneManagementModel;
        private readonly LoadingScreenModel _loadingScreenModel;
        private readonly SerialDisposable _loadingScreenVisibilityDisposer;

        public SceneManagementController(
            SceneManagementModel sceneManagementModel,
            LoadingScreenModel loadingScreenModel)
        {
            _sceneManagementModel = sceneManagementModel;
            _loadingScreenModel = loadingScreenModel;
            _loadingScreenVisibilityDisposer = new SerialDisposable().AddTo(Disposer);

            _loadingScreenModel.OnCloseLoadingScreenCompleted
                .Subscribe(_ => _sceneManagementModel.PublishOnSceneStarted())
                .AddTo(Disposer);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            var currentScene = (Scenes)Enum.Parse(typeof(Scenes), scene.name);
            _sceneManagementModel.SetCurrentScene(currentScene);
        }

        private void LoadScene(Scenes sceneToLoad)
        {
            _loadingScreenVisibilityDisposer.Disposable?.Dispose();

            var sceneName = sceneToLoad.ToString();
            SceneManager.LoadSceneAsync(sceneName);
        }

        public void OnSceneInitializationCompleted()
        {
            _loadingScreenModel.SetIsLoadingScreenVisible(false);
        }

        public void ToTitle()
        {
            _loadingScreenModel.SetIsLoadingScreenVisible(true);

            _loadingScreenVisibilityDisposer.Disposable = _loadingScreenModel.OnOpenLoadingScreenCompleted
                .Subscribe(_ => LoadScene(Scenes.TitleScene));
        }

        public void ToGame()
        {
            _loadingScreenModel.SetIsLoadingScreenVisible(true);

            _loadingScreenVisibilityDisposer.Disposable = _loadingScreenModel.OnOpenLoadingScreenCompleted
                .Subscribe(_ => LoadScene(Scenes.GameScene));
        }
    }
}
