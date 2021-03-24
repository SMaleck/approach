using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
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

        private IAvatarLocator _avatarLocator;
        private IGameRoundStateModel _gameRoundStateModel;
        private HealthDataComponent _healthDataComponent;

        [Inject]
        private void Inject(
            IAvatarLocator avatarLocator,
            IGameRoundStateModel gameRoundStateModel)
        {
            _avatarLocator = avatarLocator;
            _gameRoundStateModel = gameRoundStateModel;
        }

        public void Initialize()
        {
            _healthDataComponent = _avatarLocator.AvatarActorStateModel.Get<HealthDataComponent>();

            _gameRoundStateModel.RemainingSeconds
                .Subscribe(OnRemainingSecondsChanged)
                .AddTo(Disposer);

            _healthDataComponent.Health
                .Subscribe(OnHealthChanged)
                .AddTo(Disposer);
        }

        private void OnRemainingSecondsChanged(double seconds)
        {
            _survivalTimeText.text = TextService.TimeFromSeconds(seconds);
        }

        private void OnHealthChanged(int health)
        {
            _healthText.text = TextService.HealthAmount(health);
        }
    }
}
