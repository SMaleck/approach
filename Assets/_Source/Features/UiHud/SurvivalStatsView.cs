using _Source.Entities.Avatar;
using _Source.Services.Texts;
using _Source.Util;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.UiHud
{
    public class SurvivalStatsView : AbstractView, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, SurvivalStatsView> { }

        [SerializeField] private TextMeshProUGUI _survivalTimeText;
        [SerializeField] private TextMeshProUGUI _healthText;

        private IAvatarStateModel _avatarStateModel;

        [Inject]
        private void Inject(IAvatarStateModel survivalStatsModel)
        {
            _avatarStateModel = survivalStatsModel;
        }
        
        public void Initialize()
        {
            _avatarStateModel.SurvivalSeconds
                .Subscribe(OnSurvivalSecondsChanged)
                .AddTo(Disposer);

            _avatarStateModel.Health
                .Subscribe(OnHealthChanged)
                .AddTo(Disposer);
        }

        private void OnSurvivalSecondsChanged(double seconds)
        {
            _survivalTimeText.text = TextService.TimeFromSeconds(seconds);
        }

        private void OnHealthChanged(double health)
        {
            _healthText.text = TextService.HealthAmount(health);
        }
    }
}
