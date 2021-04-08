using _Source.Features.PlayerStatistics.Savegame;
using _Source.Util;
using UniRx;

namespace _Source.Features.PlayerStatistics
{
    public class PlayerStatisticsModel : AbstractDisposableFeature, IPlayerStatisticsModel
    {
        private readonly IPlayerStatisticsSavegame _savegame;

        public IReadOnlyReactiveProperty<int> RoundsPlayed => _savegame.RoundsPlayed;

        public PlayerStatisticsModel(IPlayerStatisticsSavegame savegame)
        {
            _savegame = savegame;
        }

        public void IncrementRoundsPlayed()
        {
            _savegame.RoundsPlayed.Value += 1;
        }
    }
}
