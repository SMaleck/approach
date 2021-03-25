using _Source.Util;
using UniRx;

namespace _Source.Features.PlayerStatistics
{
    // ToDo V2 Store statistics in savegame
    public class PlayerStatisticsModel : AbstractDisposable
    {
        private readonly IReactiveProperty<int> _friends;
        public IReadOnlyReactiveProperty<int> Friends => _friends;

        private readonly IReactiveProperty<int> _enemies;
        public IReadOnlyReactiveProperty<int> Enemies => _enemies;

        private readonly IReactiveProperty<int> _neutral;
        public IReadOnlyReactiveProperty<int> Neutral => _neutral;

        private readonly IReactiveProperty<int> _friendsLost;
        public IReadOnlyReactiveProperty<int> FriendsLost => _friendsLost;
        
        // ToDo V1 Show on round end
        public PlayerStatisticsModel()
        {
            _friends = new ReactiveProperty<int>().AddTo(Disposer);
            _enemies = new ReactiveProperty<int>().AddTo(Disposer);
            _neutral = new ReactiveProperty<int>().AddTo(Disposer);
            _friendsLost = new ReactiveProperty<int>().AddTo(Disposer);
        }

        public void IncrementFriends()
        {
            _friends.Value++;
        }

        public void IncrementEnemies()
        {
            _enemies.Value++;
        }

        public void IncrementNeutral()
        {
            _neutral.Value++;
        }

        public void IncrementFriendsLost()
        {
            _friendsLost.Value++;
        }
    }
}
