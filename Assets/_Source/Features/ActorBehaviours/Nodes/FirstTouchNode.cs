using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Data;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Sensors;
using BehaviourTreeSystem;
using System.Linq;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FirstTouchNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, FirstTouchNode> { }

        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private readonly SensorDataComponent _sensorDataComponent;
        private readonly RelationshipDataComponent _relationshipDataComponent;

        public FirstTouchNode(
            IActorStateModel actorStateModel,
            BehaviourTreeConfig behaviourTreeConfig)
        {
            _behaviourTreeConfig = behaviourTreeConfig;

            _sensorDataComponent = actorStateModel.Get<SensorDataComponent>();
            _relationshipDataComponent = actorStateModel.Get<RelationshipDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (_relationshipDataComponent.Relationship.Value != EntityState.Unacquainted)
            {
                return BehaviourTreeStatus.Failure;
            }
            if (HasTouchedValidActor())
            {
                RollRelationShip();
                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }

        private bool HasTouchedValidActor()
        {
            if (_sensorDataComponent.IsAvatarInRange(SensorType.Touch))
            {
                return true;
            }

            if (_sensorDataComponent
                .GetInRange(SensorType.Touch)
                .Any(IsFriend))
            {
                return true;
            }

            return false;
        }

        private bool IsFriend(IActorStateModel actor)
        {
            return actor.Get<RelationshipDataComponent>()?.Relationship.Value == EntityState.Friend;
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
