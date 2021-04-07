using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using System;
using UniRx;
using Zenject;

namespace _Source.Features.GameRound.Duration
{
    public class GameRoundDurationController : AbstractDisposable, IInitializable
    {
        private readonly IGameRoundDurationModel _gameRoundDurationModel;
        private readonly IAvatarLocator _avatarLocator;
        private readonly IGameRoundStateModel _gameGameRoundStateModel;
        private readonly IPauseStateModel _pauseStateModel;

        public GameRoundDurationController(
            IGameRoundDurationModel gameRoundDurationModel,
            IAvatarLocator avatarLocator,
            IGameRoundStateModel gameGameRoundStateModel,
            IPauseStateModel pauseStateModel)
        {
            _gameRoundDurationModel = gameRoundDurationModel;
            _avatarLocator = avatarLocator;
            _gameGameRoundStateModel = gameGameRoundStateModel;
            _pauseStateModel = pauseStateModel;
        }

        public void Initialize()
        {
            Observable.Merge(
                    _gameGameRoundStateModel.OnRoundStarted,
                    _avatarLocator.IsAvatarSpawned.AsUnitObservable())
                .Where(_ => _avatarLocator.IsAvatarSpawned.Value)
                .Subscribe(_ => Start())
                .AddTo(Disposer);
        }

        private void Start()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Where(_ => !_pauseStateModel.IsPaused.Value)
                .Subscribe(_ => OnTick())
                .AddTo(Disposer);

            var healthDataComponent = _avatarLocator.AvatarActor.Get<HealthDataComponent>();
            healthDataComponent.Health
                .Pairwise()
                .Where(p => p.Current < p.Previous)
                .Subscribe(OnHealthLoss);
        }

        private void OnTick()
        {
            var seconds = _gameRoundDurationModel.RemainingSeconds.Value - 1;
            _gameRoundDurationModel.Tick();
        }

        private void OnHealthLoss(Pair<int> healthPair)
        {
            _gameRoundDurationModel.DeductSecondsForHealth(
                healthPair.Previous - healthPair.Current);
        }
    }
}
