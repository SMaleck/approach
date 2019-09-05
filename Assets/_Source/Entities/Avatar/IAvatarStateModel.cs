using System;
using UniRx;

namespace _Source.Entities.Avatar
{
    public interface IAvatarStateModel
    {
        IReadOnlyReactiveProperty<DateTime> StartedAt { get; }
        IReadOnlyReactiveProperty<double> SurvivalSeconds { get; }
        IReadOnlyReactiveProperty<double> Health { get; }
        IReadOnlyReactiveProperty<bool> IsAlive { get; }
    }
}