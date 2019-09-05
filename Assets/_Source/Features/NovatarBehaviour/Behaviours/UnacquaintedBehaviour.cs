using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour.Data;
using Assets._Source.Entities.Novatar;
using FluentBehaviourTree;
using System.Linq;
using UniRx;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Behaviours
{
    public class UnacquaintedBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, UnacquaintedBehaviour> { }

        private readonly IAvatar _avatarEntity;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private readonly IBehaviourTreeNode _behaviourTree;

        private double _timePassedForStatusEvaluation;

        public UnacquaintedBehaviour(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            IAvatar avatarEntity,
            BehaviourTreeConfig behaviourTreeConfig)
            : base(novatarEntity, novatarStateModel)
        {
            _avatarEntity = avatarEntity;
            _behaviourTreeConfig = behaviourTreeConfig;

            _behaviourTree = CreateTree();

            NovatarStateModel.OnReset
                .Subscribe(_ => Reset())
                .AddTo(Disposer);
        }

        private void Reset()
        {
            _timePassedForStatusEvaluation = 0;
        }

        public override IBehaviourTreeNode Build()
        {
            return _behaviourTree;
        }

        private IBehaviourTreeNode CreateTree()
        {
            return new BehaviourTreeBuilder()
                .Selector(nameof(UnacquaintedBehaviour))
                    .Sequence("FollowAvatar")
                        .Condition(nameof(IsInFollowRange), t => IsInFollowRange())
                        .Do(nameof(FollowAvatar), t => FollowAvatar())
                        .End()
                    .Sequence("TouchAvatar")
                        .Condition(nameof(IsInTouchRange), t => IsInTouchRange())
                        .Do(nameof(EvaluateRelationshipOnTouch), t => EvaluateRelationshipOnTouch())
                        .End()
                    .Do(nameof(EvaluateRelationshipOnTime), EvaluateRelationshipOnTime)
                .End()
                .Build();
        }

        private BehaviourTreeStatus FollowAvatar()
        {
            _timePassedForStatusEvaluation = 0;

            NovatarEntity.MoveTowards(_avatarEntity.Position);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus EvaluateRelationshipOnTouch()
        {
            EntityState nextStatus = GetWeightedRandomRelationshipStatus();

            NovatarEntity.SwitchToEntityState(nextStatus);
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

            return NovatarStateModel.CurrentEntityState.Value;
        }

        private BehaviourTreeStatus EvaluateRelationshipOnTime(TimeData timeData)
        {
            _timePassedForStatusEvaluation += timeData.deltaTime;

            var timeoutSeconds = _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds;

            if (_timePassedForStatusEvaluation < timeoutSeconds)
            {
                return BehaviourTreeStatus.Success;
            }

            // Switch to NEUTRAL based on Dice Roll
            var switchChance = _behaviourTreeConfig.UnacquaintedConfig.TimeBasedSwitchChance;
            var diceRoll = UnityEngine.Random.Range(0f, 1f);

            if (diceRoll <= switchChance)
            {
                NovatarEntity.SwitchToEntityState(EntityState.Neutral);
            }
            else
            {
                // Reset interval, if this switch-roll failed
                _timePassedForStatusEvaluation = 0;
            }

            return BehaviourTreeStatus.Success;
        }
    }
}
