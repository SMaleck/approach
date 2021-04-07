using _Source.Entities.Novatar;
using _Source.Util;

namespace _Source.Features.PlayerStatistics
{
    public class GameRoundStatisticsController : AbstractDisposable
    {
        private readonly IGameRoundStatisticsModel _gameRoundStatisticsModel;

        public GameRoundStatisticsController(IGameRoundStatisticsModel gameRoundStatisticsModel)
        {
            _gameRoundStatisticsModel = gameRoundStatisticsModel;
        }

        public void RegisterRelationshipSwitch(EntityState from, EntityState to)
        {
            switch (to)
            {
                case EntityState.Neutral:
                    _gameRoundStatisticsModel.IncrementNeutral();
                    if (from == EntityState.Friend)
                    {
                        _gameRoundStatisticsModel.IncrementFriendsLost();
                    }
                    break;

                case EntityState.Enemy:
                    _gameRoundStatisticsModel.IncrementEnemies();
                    break;

                case EntityState.Friend:
                    _gameRoundStatisticsModel.IncrementFriends();
                    break;
            }
        }
    }
}
