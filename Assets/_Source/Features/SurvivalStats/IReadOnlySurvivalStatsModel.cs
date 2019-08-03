using System;
using UniRx;

namespace _Source.Features.SurvivalStats
{
    public interface IReadOnlySurvivalStatsModel
    {
        IReadOnlyReactiveProperty<DateTime> StartedAt { get; }
        IReadOnlyReactiveProperty<double> SurvivalSeconds { get; }
    }
}