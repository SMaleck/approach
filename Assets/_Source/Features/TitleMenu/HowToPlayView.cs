using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Source.Services.Texts;
using _Source.Util;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using UniRx.Triggers;

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

        private Tween _tutorialTween;

        public void Initialize()
        {
            _tutorialTween = CreateTutorialTween();

            gameObject.OnEnableAsObservable()
                .Subscribe(_ => OnObservableEnable())
                .AddTo(Disposer);
        }

        private Tween CreateTutorialTween()
        {
            var tween = DOTween.Sequence()
                .Append(_tutorialStepOneParent.DOFade(1, 1))
                .AppendInterval(1)
                .Append(_tutorialStepOneParent.DOFade(1, 1))
                .Pause()
                .SetAutoKill(false);

            return tween;
        }

        private void OnObservableEnable()
        {
            _tutorialStepOneParent.alpha = 0;
            _tutorialStepTwoParent.alpha = 0;

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
