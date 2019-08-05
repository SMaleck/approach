﻿using _Source.Util;
using UniRx;

namespace _Source.Features.GameRound
{
    public class GameRoundModel : AbstractDisposable
    {
        private readonly ReactiveProperty<bool> _isPaused;
        private IReadOnlyReactiveProperty<bool> IsPaused => _isPaused;

        private readonly ReactiveProperty<bool> _isInProgress;
        private IReadOnlyReactiveProperty<bool> IsInProgress => _isInProgress;

        private readonly Subject<Unit> _onRoundStarted;
        public IOptimizedObservable<Unit> OnRoundStarted => _onRoundEnded;

        private readonly Subject<Unit> _onRoundEnded;
        public IOptimizedObservable<Unit> OnRoundEnded => _onRoundEnded;

        public GameRoundModel()
        {
            _isPaused = new ReactiveProperty<bool>().AddTo(Disposer);
            _isInProgress = new ReactiveProperty<bool>().AddTo(Disposer);
            _onRoundStarted = new Subject<Unit>().AddTo(Disposer);
            _onRoundEnded = new Subject<Unit>().AddTo(Disposer);
        }

        public void SetIsPaused(bool value)
        {
            _isPaused.Value = value;
        }

        private void SetIsInProgress(bool value)
        {
            _isInProgress.Value = value;
        }

        public void PublishOnRoundStarted()
        {
            _onRoundStarted.OnNext(Unit.Default);
            SetIsInProgress(true);
        }

        public void PublishOnRoundEnded()
        {
            _onRoundEnded.OnNext(Unit.Default);
            SetIsInProgress(false);
        }
    }
}
