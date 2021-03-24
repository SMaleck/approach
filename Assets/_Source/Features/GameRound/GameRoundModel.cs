using _Source.Util;
using System;
using UniRx;

namespace _Source.Features.GameRound
{
    public class GameRoundModel : AbstractDisposable, IGameRoundStateModel, IPauseStateModel
    {
        public double RoundTimeSeconds => 180;

        private readonly ReactiveProperty<bool> _isPaused;
        public IReadOnlyReactiveProperty<bool> IsPaused => _isPaused;

        private readonly ReactiveProperty<double> _remainingSeconds;
        public IReadOnlyReactiveProperty<double> RemainingSeconds => _remainingSeconds;

        private readonly Subject<Unit> _onRoundStarted;
        public IOptimizedObservable<Unit> OnRoundStarted => _onRoundEnded;

        private readonly Subject<Unit> _onRoundEnded;
        public IOptimizedObservable<Unit> OnRoundEnded => _onRoundEnded;

        public GameRoundModel()
        {
            _isPaused = new ReactiveProperty<bool>().AddTo(Disposer);
            _remainingSeconds = new ReactiveProperty<double>().AddTo(Disposer);
            _onRoundStarted = new Subject<Unit>().AddTo(Disposer);
            _onRoundEnded = new Subject<Unit>().AddTo(Disposer);
        }

        public void SetIsPaused(bool value)
        {
            _isPaused.Value = value;
        }

        public void PublishOnRoundStarted()
        {
            _onRoundStarted.OnNext(Unit.Default);
        }

        public void PublishOnRoundEnded()
        {
            _onRoundEnded.OnNext(Unit.Default);
        }

        public void SetRemainingSeconds(double seconds)
        {
            _remainingSeconds.Value = Math.Max(0, seconds);
        }
    }
}
