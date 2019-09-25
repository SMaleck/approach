using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour.Data;
using _Source.Features.NovatarBehaviour.Sensors;
using FluentBehaviourTree;
using System.Linq;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class FirstTouchNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, ISensorySystem, FirstTouchNode> { }

        private readonly INovatar _novatarEntity;
        private readonly INovatarStateModel _novatarStateModel;
        private readonly ISensorySystem _sensorySystem;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        public FirstTouchNode(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            ISensorySystem sensorySystem,
            BehaviourTreeConfig behaviourTreeConfig)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _sensorySystem = sensorySystem;
            _behaviourTreeConfig = behaviourTreeConfig;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_sensorySystem.IsInTouchRange())
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
