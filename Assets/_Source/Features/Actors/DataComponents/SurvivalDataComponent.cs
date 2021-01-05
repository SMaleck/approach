using _Source.Util;
using System;
using UniRx;
using Zenject;

namespace _Source.Entities.Actors.DataComponents
{
    public class SurvivalDataComponent : AbstractDisposableFeature, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<SurvivalDataComponent> { }

        private readonly ReactiveProperty<DateTime> _startedAt;
        public IReadOnlyReactiveProperty<DateTime> StartedAt => _startedAt;

        private readonly ReactiveProperty<double> _survivalSeconds;
        public IReadOnlyReactiveProperty<double> SurvivalSeconds => _survivalSeconds;

        public SurvivalDataComponent()
        {
            _startedAt = new ReactiveProperty<DateTime>(DateTime.Now).AddTo(Disposer);
            _survivalSeconds = new ReactiveProperty<double>().AddTo(Disposer);
        }

        public void SetStartedAt(DateTime value)
        {
            _startedAt.Value = value;
        }

        public void SetSurvivalSeconds(double value)
        {
            _survivalSeconds.Value = value;
        }

        public void Reset()
        {
            SetStartedAt(DateTime.Now);
            SetSurvivalSeconds(0);
        }
    }
}
