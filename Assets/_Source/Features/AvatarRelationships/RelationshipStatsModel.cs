using _Source.Util;
using UniRx;

namespace _Source.Features.AvatarRelationships
{
    public class RelationshipStatsModel : AbstractDisposable, IRelationshipStatsModel
    {
        private readonly ReactiveProperty<int> _encountersCount;
        public IReadOnlyReactiveProperty<int> EncountersCount => _encountersCount;

        private readonly ReactiveProperty<int> _friendsGainedCount;
        public IReadOnlyReactiveProperty<int> FriendsGainedCount => _friendsGainedCount;

        private readonly ReactiveProperty<int> _friendsLostCount;
        public IReadOnlyReactiveProperty<int> FriendsLostCount => _friendsLostCount;

        private readonly ReactiveProperty<int> _enemiesGainedCount;
        public IReadOnlyReactiveProperty<int> EnemiesGainedCount => _enemiesGainedCount;

        public RelationshipStatsModel()
        {
            _encountersCount = new ReactiveProperty<int>().AddTo(Disposer);
            _friendsGainedCount = new ReactiveProperty<int>().AddTo(Disposer);
            _friendsLostCount = new ReactiveProperty<int>().AddTo(Disposer);
            _enemiesGainedCount = new ReactiveProperty<int>().AddTo(Disposer);
        }

        public void IncrementEncountersCount()
        {
            _encountersCount.Value++;
        }

        public void IncrementFriendsGainedCount()
        {
            _friendsGainedCount.Value++;
        }

        public void IncrementFriendsLostCount()
        {
            _friendsLostCount.Value++;
        }

        public void IncrementEnemiesGainedCount()
        {
            _enemiesGainedCount.Value++;
        }
    }
}
