using _Source.Services.Texts;
using _Source.Util;
using DG.Tweening;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace _Source.Features.UiScreens
{
    public class HowToPlayView : AbstractView, IInitializable, ILocalizable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, HowToPlayView> { }

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _howToControls;
        [SerializeField] private TextMeshProUGUI _howToStep1;
        [SerializeField] private TextMeshProUGUI _howToStep2;
        [SerializeField] private TextMeshProUGUI _howToStep3;

        [Header("Parents")]
        [SerializeField] private CanvasGroup _howToControlsParent;
        [SerializeField] private CanvasGroup _howToStep1Parent;
        [SerializeField] private CanvasGroup _howToStep2Parent;
        [SerializeField] private CanvasGroup _howToStep3Parent;

        [Header("Fading")]
        [SerializeField] private float _stepFadeInSeconds;
        [SerializeField] private float _stepFadeInDelaySeconds;

        private Tween _tutorialTween;

        public void Initialize()
        {
            _tutorialTween = CreateTutorialTween();

            gameObject.OnEnableAsObservable()
                .Subscribe(_ => OnObservableEnable())
                .AddTo(Disposer);

            Localize();
        }

        private Tween CreateTutorialTween()
        {
            _howToControlsParent.alpha = 0;
            _howToStep1Parent.alpha = 0;
            _howToStep2Parent.alpha = 0;
            _howToStep3Parent.alpha = 0;

            var tween = DOTween.Sequence()
                .Append(_howToStep1Parent.DOFade(1, _stepFadeInSeconds))
                .AppendInterval(_stepFadeInDelaySeconds)
                .Append(_howToControlsParent.DOFade(1, _stepFadeInSeconds))
                .AppendInterval(_stepFadeInDelaySeconds)
                .Append(_howToStep2Parent.DOFade(1, _stepFadeInSeconds))
                .AppendInterval(_stepFadeInDelaySeconds)
                .Append(_howToStep3Parent.DOFade(1, _stepFadeInSeconds))
                .Pause()
                .SetAutoKill(false);

            return tween;
        }

        private void OnObservableEnable()
        {
            _tutorialTween.Restart();
        }

        public void Localize()
        {
            _titleText.text = TextService.HowToPlay();
            _howToControls.text = TextService.HowToControls();
            _howToStep1.text = TextService.HowToStep1();
            _howToStep2.text = TextService.HowToStep2();
            _howToStep3.text = TextService.HowToStep3();
        }
    }
}
