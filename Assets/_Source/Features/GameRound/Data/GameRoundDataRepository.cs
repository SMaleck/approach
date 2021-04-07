using _Source.Data;

namespace _Source.Features.GameRound.Data
{
    public class GameRoundDataRepository : AbstractDataRepository, IGameRoundData
    {
        private readonly GameRoundDataEntry _data;

        public double DurationSeconds => _data.DurationSeconds;
        public double SecondsPerHP => _data.SecondsPerHP;

        public GameRoundDataRepository(GameRoundDataSource dataSource)
        {
            _data = dataSource.DataEntries[0];
        }
    }
}
