using UniRx;

namespace _Source.Features.GameRound
{
    public interface IGameRoundStateModel
    {
        IOptimizedObservable<Unit> OnRoundStarted { get; }
        IOptimizedObservable<Unit> OnRoundEnded { get; }
    }
}