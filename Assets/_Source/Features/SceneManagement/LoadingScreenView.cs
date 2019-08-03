using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.SceneManagement
{
    public class LoadingScreenView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, LoadingScreenView> { }

        [SerializeField] private CanvasGroup _canvasGroup;

        private SceneManagementModel _sceneManagementModel;
        //private Tween _openTween;

        [Inject]
        private void Inject(SceneManagementModel sceneManagementModel)
        {
            _sceneManagementModel = sceneManagementModel;
        }

        public void Initialize()
        {
            _sceneManagementModel.IsLoadingScreenVisible
                .Subscribe(UpdateVisibility)
                .AddTo(Disposer);
        }

        private void UpdateVisibility(bool isVisible)
        {
            if (!isVisible)
            {
                Close();
                return;
            }

            Open();
        }
    }
}
