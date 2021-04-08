using _Source.Features.FeatureToggles;
using UniRx;

namespace _Source.Features.GameRound.Duration
{
    public interface IGameRoundDurationModel : ITogglableFeature
    {
        IReadOnlyReactiveProperty<double> RemainingSeconds { get; }
        IReadOnlyReactiveProperty<bool> IsRunning { get; }
        IReadOnlyReactiveProperty<double> Progress { get; }

        void Tick();
        void SetRemainingSeconds(double seconds);
        void DeductSecondsForHealth(int health);
    }
}