using UniRx;

namespace _Source.Features.PlayerStatistics.Savegame
{
    public interface IPlayerStatisticsSavegame
    {
        IReactiveProperty<int> RoundsPlayed { get; }
    }
}