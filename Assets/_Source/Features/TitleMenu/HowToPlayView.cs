using _Source.Services.Texts;
using _Source.Util;
using DG.Tweening;
using System.Linq;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Features.TitleMenu
{
    public class HowToPlayView : AbstractView, IInitializable, ILocalizable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, HowToPlayView> { }

        [Header("Close Button")]
        [SerializeField] private Button _closeButton;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _tutorialStepOne;
        [SerializeField] private TextMeshProUGUI _tutorialStepTwo;

        [Header("Parents")]
        [SerializeField] private CanvasGroup _tutorialStepOneParent;
        [SerializeField] private CanvasGroup _tutorialStepTwoParent;

        [Header("Fading")]
        [SerializeField] private float _stepFadeInSeconds;
        [SerializeField] private float _stepFadeInDelaySeconds;

        private Tween _tutorialTween;

        public void Initialize()
        {
            _closeButton.OnClickAsObservable()
                .Subscribe(_ => Close())
                .AddTo(Disposer);

            _tutorialTween = CreateTutorialTween();

            gameObject.OnEnableAsObservable()
                .Subscribe(_ => OnObservableEnable())
                .AddTo(Disposer);
        }

        private Tween CreateTutorialTween()
        {
            _tutorialStepOneParent.alpha = 0;
            _tutorialStepTwoParent.alpha = 0;

            var tween = DOTween.Sequence()
                .Append(_tutorialStepOneParent.DOFade(1, _stepFadeInSeconds))
                .AppendInterval(_stepFadeInDelaySeconds)
                .Append(_tutorialStepTwoParent.DOFade(1, _stepFadeInSeconds))
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
            _tutorialStepOne.text = TextService.TutorialStepOne();
            _tutorialStepTwo.text = TextService.TutorialStepTwo();
        }
    }
}
