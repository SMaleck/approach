using UniRx;

namespace _Source.Features.GameRound.Duration
{
    public interface IGameRoundDurationModel
    {
        IReadOnlyReactiveProperty<double> RemainingSeconds { get; }
        IReadOnlyReactiveProperty<bool> IsRunning { get; } 
        
        void Tick();
        void SetRemainingSeconds(double seconds);
        void DeductSecondsForHealth(int health);
    }
}