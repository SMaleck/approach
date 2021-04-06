using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using System;
using UniRx;
using Zenject;

namespace _Source.Features.GameRound
{
    // ToDo V1 Track amount of Relationships and show result
    public class GameRoundController : AbstractDisposable, IInitializable
    {
        private readonly GameRoundModel _gameRoundModel;
        private readonly IAvatarLocator _avatarLocator;
        private IDisposable _timerDisposer;

        public GameRoundController(
            GameRoundModel gameRoundModel,
            IAvatarLocator avatarLocator)
        {
            _gameRoundModel = gameRoundModel;
            _avatarLocator = avatarLocator;

            PauseRound(true);
        }

        public void Initialize()
        {
            var healthDataComponent = _avatarLocator.AvatarActor.Get<HealthDataComponent>();
            healthDataComponent.IsAlive
                .Where(isAlive => !isAlive)
                .Take(1)
                .Subscribe(_ => EndRound())
                .AddTo(Disposer);

            _gameRoundModel.SetRemainingSeconds(_gameRoundModel.RoundTimeSeconds);
            _timerDisposer = Observable.Interval(TimeSpan.FromSeconds(1))
                .Where(_ => !_gameRoundModel.IsPaused.Value)
                .Subscribe(_ => UpdateRoundTime())
                .AddTo(Disposer);
        }

        public void PauseRound(bool isPaused)
        {
            _gameRoundModel.SetIsPaused(isPaused);
        }

        public void StartRound()
        {
            PauseRound(false);
            _gameRoundModel.PublishOnRoundStarted();
        }

        private void UpdateRoundTime()
        {
            var seconds = _gameRoundModel.RemainingSeconds.Value - 1;
            _gameRoundModel.SetRemainingSeconds(seconds);

            if (_gameRoundModel.RemainingSeconds.Value <= 0)
            {
                EndRound();
            }
        }

        private void EndRound()
        {
            _timerDisposer?.Dispose();
            _gameRoundModel.PublishOnRoundEnded();
        }
    }
}
