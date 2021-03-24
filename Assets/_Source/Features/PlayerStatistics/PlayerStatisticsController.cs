using _Source.Entities.Novatar;
using _Source.Util;

namespace _Source.Features.PlayerStatistics
{
    public class PlayerStatisticsController : AbstractDisposable
    {
        private readonly PlayerStatisticsModel _playerStatisticsModel;

        public PlayerStatisticsController(PlayerStatisticsModel playerStatisticsModel)
        {
            _playerStatisticsModel = playerStatisticsModel;
        }

        public void RegisterRelationshipSwitch(EntityState from, EntityState to)
        {
            switch (to)
            {
                case EntityState.Neutral:
                    _playerStatisticsModel.IncrementNeutral();
                    if (from == EntityState.Friend)
                    {
                        _playerStatisticsModel.IncrementFriendsLost();
                    }
                    break;

                case EntityState.Enemy:
                    _playerStatisticsModel.IncrementEnemies();
                    break;

                case EntityState.Friend:
                    _playerStatisticsModel.IncrementFriends();
                    break;
            }
        }
    }
}
