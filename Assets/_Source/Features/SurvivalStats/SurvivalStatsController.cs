using _Source.Util;
using System;
using UniRx;

namespace _Source.Features.SurvivalStats
{
    public class SurvivalStatsController : AbstractDisposable
    {
        private readonly SurvivalStatsModel _survivalStatsModel;

        public SurvivalStatsController(SurvivalStatsModel survivalStatsModel)
        {
            _survivalStatsModel = survivalStatsModel;

            _survivalStatsModel.SetStartedAt(DateTime.Now);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ => OnTimePassed())
                .AddTo(Disposer);
        }

        private void OnTimePassed()
        {
            var timePassed = DateTime.Now - _survivalStatsModel.StartedAt.Value;
            _survivalStatsModel.SetSurvivalSeconds(timePassed.TotalSeconds);
        }
    }
}
