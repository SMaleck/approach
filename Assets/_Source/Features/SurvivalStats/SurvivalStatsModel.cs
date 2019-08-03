using System;
using _Source.Util;
using UniRx;

namespace _Source.Features.SurvivalStats
{
    public class SurvivalStatsModel : AbstractDisposable, IReadOnlySurvivalStatsModel
    {
        private readonly ReactiveProperty<DateTime> _startedAt;
        public IReadOnlyReactiveProperty<DateTime> StartedAt => _startedAt;

        private readonly ReactiveProperty<double> _survivalSeconds;
        public IReadOnlyReactiveProperty<double> SurvivalSeconds => _survivalSeconds;

        private readonly ReactiveProperty<bool> _isAlive;
        public IReadOnlyReactiveProperty<bool> IsAlive => _isAlive;

        public SurvivalStatsModel()
        {
            _startedAt = new ReactiveProperty<DateTime>().AddTo(Disposer);
            _survivalSeconds = new ReactiveProperty<double>().AddTo(Disposer);
            _isAlive = new ReactiveProperty<bool>(true).AddTo(Disposer);
        }

        public void SetStartedAt(DateTime value)
        {
            _startedAt.Value = value;
        }

        public void SetSurvivalSeconds(double value)
        {
            _survivalSeconds.Value = value;
        }

        public void SetIsAlive(bool value)
        {
            _isAlive.Value = value;
        }
    }
}
