using System.Linq;
using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Data;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ActorSensors;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FirstTouchNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, ISensorySystem, FirstTouchNode> { }

        private readonly ISensorySystem _sensorySystem;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private readonly RelationshipDataComponent _relationshipDataComponent;

        public FirstTouchNode(
            IActorStateModel actorStateModel,
            ISensorySystem sensorySystem,
            BehaviourTreeConfig behaviourTreeConfig)
        {
            _sensorySystem = sensorySystem;
            _behaviourTreeConfig = behaviourTreeConfig;

            _relationshipDataComponent = actorStateModel.Get<RelationshipDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_sensorySystem.IsInTouchRange())
            {
                return BehaviourTreeStatus.Failure;
            }

            EntityState nextStatus = GetWeightedRandomRelationshipStatus();
            _relationshipDataComponent.SetRelationship(nextStatus);

            return BehaviourTreeStatus.Success;
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
