using _Source.Features.GameRound.Data;
using _Source.Util;
using System;
using _Source.Features.FeatureToggles;
using UniRx;

namespace _Source.Features.GameRound.Duration
{
    public class GameRoundDurationModel : AbstractDisposable, IGameRoundDurationModel
    {
        private readonly IGameRoundData _data;
        private double DurationSeconds => _data.DurationSeconds;

        private readonly ReactiveProperty<double> _remainingSeconds;
        public IReadOnlyReactiveProperty<double> RemainingSeconds => _remainingSeconds;

        public IReadOnlyReactiveProperty<bool> IsRunning { get; }
        public IReadOnlyReactiveProperty<double> Progress { get; }

        public FeatureId FeatureId => FeatureId.GameRoundTime;

        private readonly IReactiveProperty<bool> _isEnabled;
        public IReadOnlyReactiveProperty<bool> IsEnabled => _isEnabled;

        public GameRoundDurationModel(IGameRoundData data)
        {
            _data = data;
            _remainingSeconds = new ReactiveProperty<double>(DurationSeconds).AddTo(Disposer);
            _isEnabled = new ReactiveProperty<bool>(true).AddTo(Disposer);

            IsRunning = _remainingSeconds
                .Select(seconds => seconds > 0)
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposer);

            Progress = _remainingSeconds
                .Select(seconds => seconds / DurationSeconds)
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposer);
        }

        public void Tick()
        {
            SetRemainingSeconds(_remainingSeconds.Value - 1);
        }

        public void SetRemainingSeconds(double seconds)
        {
            _remainingSeconds.Value = Math.Max(0, seconds);
        }

        public void DeductSecondsForHealth(int health)
        {
            var deduction = _data.SecondsPerHP * health;
            SetRemainingSeconds(_remainingSeconds.Value - deduction);
        }

        public void SetIsEnabled(bool isEnabled)
        {
            _isEnabled.Value = isEnabled;
        }
    }
}
