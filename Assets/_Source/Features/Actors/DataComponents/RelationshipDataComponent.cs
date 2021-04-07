using _Source.Entities.Novatar;
using _Source.Features.PlayerStatistics;
using UniRx;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class RelationshipDataComponent : AbstractDataComponent, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<RelationshipDataComponent> { }

        private readonly GameRoundStatisticsController _gameRoundStatisticsController;

        private readonly ReactiveProperty<EntityState> _relationship;
        public IReadOnlyReactiveProperty<EntityState> Relationship => _relationship;

        public RelationshipDataComponent(GameRoundStatisticsController gameRoundStatisticsController)
        {
            _gameRoundStatisticsController = gameRoundStatisticsController;
            _relationship = new ReactiveProperty<EntityState>()
                .AddTo(Disposer);

            Reset();
        }

        public void SetRelationship(EntityState value)
        {
            _gameRoundStatisticsController.RegisterRelationshipSwitch(
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
