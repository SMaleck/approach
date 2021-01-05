using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
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

        private SurvivalDataComponent _survivalDataComponent;
        private HealthDataComponent _healthDataComponent;

        [Inject]
        private void Inject(IActorStateModel actorStateModel)
        {
            _survivalDataComponent = actorStateModel.Get<SurvivalDataComponent>();
            _healthDataComponent = actorStateModel.Get<HealthDataComponent>();
        }

        public void Initialize()
        {
            _survivalDataComponent.SurvivalSeconds
                .Subscribe(OnSurvivalSecondsChanged)
                .AddTo(Disposer);

            _healthDataComponent.Health
                .Subscribe(OnHealthChanged)
                .AddTo(Disposer);
        }

        private void OnSurvivalSecondsChanged(double seconds)
        {
            _survivalTimeText.text = TextService.TimeFromSeconds(seconds);
        }

        private void OnHealthChanged(int health)
        {
            _healthText.text = TextService.HealthAmount(health);
        }
    }
}
