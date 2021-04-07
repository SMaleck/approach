using UniRx;

namespace _Source.Features.PlayerStatistics
{
    public interface IPlayerStatisticsModel
    {
        IReadOnlyReactiveProperty<int> RoundsPlayed { get; }
        void IncrementRoundsPlayed();
    }
}