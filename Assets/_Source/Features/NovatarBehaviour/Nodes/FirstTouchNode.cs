using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour.Data;
using FluentBehaviourTree;
using System.Linq;
using _Source.Features.NovatarBehaviour.Sensors;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class FirstTouchNode : IBehaviourTreeNode
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, RangeSensor, FirstTouchNode> { }

        private readonly INovatar _novatarEntity;
        private readonly INovatarStateModel _novatarStateModel;
        private readonly RangeSensor _rangeSensor;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        public FirstTouchNode(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            RangeSensor rangeSensor,
            BehaviourTreeConfig behaviourTreeConfig)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _rangeSensor = rangeSensor;
            _behaviourTreeConfig = behaviourTreeConfig;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_rangeSensor.IsInTouchRange())
            {
                return BehaviourTreeStatus.Failure;
            }

            EntityState nextStatus = GetWeightedRandomRelationshipStatus();
            _novatarEntity.SwitchToEntityState(nextStatus);

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

            return _novatarStateModel.CurrentEntityState.Value;
        }
    }
}
