using UniRx;

namespace _Source.Features.GameRound
{
    public interface IGameRoundStateModel
    {
        IReadOnlyReactiveProperty<double> RemainingSeconds { get; }
        IOptimizedObservable<Unit> OnRoundStarted { get; }
        IOptimizedObservable<Unit> OnRoundEnded { get; }
    }
}