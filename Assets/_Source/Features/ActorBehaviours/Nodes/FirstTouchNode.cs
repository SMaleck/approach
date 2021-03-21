using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Data;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using System.Linq;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FirstTouchNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, FirstTouchNode> { }

        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private readonly TransformDataComponent _transformDataComponent;
        private readonly SensorDataComponent _sensorDataComponent;
        private readonly RelationshipDataComponent _relationshipDataComponent;

        public FirstTouchNode(
            IActorStateModel actorStateModel,
            BehaviourTreeConfig behaviourTreeConfig)
        {
            _behaviourTreeConfig = behaviourTreeConfig;

            _transformDataComponent = actorStateModel.Get<TransformDataComponent>();
            _sensorDataComponent = actorStateModel.Get<SensorDataComponent>();
            _relationshipDataComponent = actorStateModel.Get<RelationshipDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (HasTouchedValidActor())
            {
                RollRelationShip();
                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }

        private bool HasTouchedValidActor()
        {
            if (_sensorDataComponent.KnowsAvatar &&
                IsInTouchRange(_sensorDataComponent.Avatar))
            {
                return true;
            }

            if (_sensorDataComponent.KnownEntities
                .Any(e => IsFriend(e) && IsInTouchRange(e)))
            {
                return true;
            }

            return false;
        }

        private bool IsFriend(IActorStateModel actor)
        {
            return actor.Get<RelationshipDataComponent>()?.Relationship.Value == EntityState.Friend;
        }

        private bool IsInTouchRange(IActorStateModel actor)
        {
            return _sensorDataComponent.IsInTouchRange(
                _transformDataComponent,
                actor.Get<TransformDataComponent>());
        }

        private void RollRelationShip()
        {
            EntityState nextStatus = GetWeightedRandomRelationshipStatus();
            _relationshipDataComponent.SetRelationship(nextStatus);
        }

        private EntityState GetWeightedRandomRelationshipStatus()
        {
            var switchChances = _behaviourTreeConfig.UnacquaintedConfig.RelationshipSwitchWeights;
            var totalWeight = switchChances.Sum(item => item.WeightedChance);
            var randomNumber = UnityEngine.Random.Range(0f, totalWeight);

            foreach (var switchChance in switchChances)
            {
                if (randomNumber < switchChance.WeightedChance)
                {
                    return switchChance.SwitchToRelationship;
                }

                randomNumber -= switchChance.WeightedChance;
            }

            return _relationshipDataComponent.Relationship.Value;
        }
    }
}
