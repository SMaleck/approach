using UniRx;

namespace _Source.Features.GameRound
{
    public interface IPauseStateModel
    {
        IReadOnlyReactiveProperty<bool> IsPaused { get; }
    }
}