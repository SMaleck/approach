using _Source.Util;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.SceneManagement
{
    public class LoadingScreenView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, LoadingScreenView> { }

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeSeconds;

        private SceneManagementModel _sceneManagementModel;
        private Tween _openTween;
        private Tween _closeTween;

        [Inject]
        private void Inject(SceneManagementModel sceneManagementModel)
        {
            _sceneManagementModel = sceneManagementModel;
        }

        public void Initialize()
        {
            _canvasGroup.alpha = 0;
            var openFade = _canvasGroup.DOFade(1, _fadeSeconds);
            openFade.ForceInit();

            _openTween = DOTween.Sequence()
                .AppendCallback(Open)
                .Append(openFade)
                .AppendCallback(OnOpenCompleted)
                .SetAutoKill(false)
                .Pause()
                .AddTo(Disposer, TweenDisposalBehaviour.Rewind);

            _canvasGroup.alpha = 1;
            var closeFade = _canvasGroup.DOFade(0, _fadeSeconds);
            closeFade.ForceInit();

            _closeTween = DOTween.Sequence()
                .AppendCallback(Open)
                .Append(closeFade)
                .AppendCallback(OnCloseCompleted)
                .SetAutoKill(false)
                .Pause()
                .AddTo(Disposer, TweenDisposalBehaviour.Rewind);

            _sceneManagementModel.IsLoadingScreenVisible
                .Subscribe(UpdateVisibility)
                .AddTo(Disposer);
        }

        private void OnOpenCompleted()
        {
            _sceneManagementModel.PublishOnOpenLoadingScreenCompleted();
        }

        private void OnCloseCompleted()
        {
            Close();
        }

        private void UpdateVisibility(bool isVisible)
        {
            if (isVisible)
            {
                _openTween.Restart();
                return;
            }

            _closeTween.Restart();
        }
    }
}
