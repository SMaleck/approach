using _Source.Entities.Novatar;
using _Source.Features.PlayerStatistics;
using UniRx;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class RelationshipDataComponent : AbstractDataComponent, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<RelationshipDataComponent> { }

        private readonly PlayerStatisticsController _playerStatisticsController;

        private readonly ReactiveProperty<EntityState> _relationship;
        public IReadOnlyReactiveProperty<EntityState> Relationship => _relationship;

        public RelationshipDataComponent(PlayerStatisticsController playerStatisticsController)
        {
            _playerStatisticsController = playerStatisticsController;
            _relationship = new ReactiveProperty<EntityState>()
                .AddTo(Disposer);

            Reset();
        }

        public void SetRelationship(EntityState value)
        {
            _playerStatisticsController.RegisterRelationshipSwitch(
                _relationship.Value, 
                value);

            _relationship.Value = value;
        }

        public void Reset()
        {
            SetRelationship(EntityState.Spawning);
        }
    }
}
