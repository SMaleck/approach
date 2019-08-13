using System;
using UniRx;

namespace _Source.Features.AvatarState
{
    public interface IReadOnlyAvatarStateModel
    {
        IReadOnlyReactiveProperty<DateTime> StartedAt { get; }
        IReadOnlyReactiveProperty<double> SurvivalSeconds { get; }
        IReadOnlyReactiveProperty<double> Health { get; }
        IReadOnlyReactiveProperty<bool> IsAlive { get; }
    }
}