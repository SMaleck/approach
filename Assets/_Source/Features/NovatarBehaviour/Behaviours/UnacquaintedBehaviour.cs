using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour.Data;
using FluentBehaviourTree;
using System.Linq;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Behaviours
{
    public class UnacquaintedBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<INovatar, NovatarStateModel, UnacquaintedBehaviour> { }

        private readonly AvatarEntity _avatar;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private readonly IBehaviourTreeNode _behaviourTree;

        public UnacquaintedBehaviour(
            INovatar novatarEntity,
            NovatarStateModel novatarStateModel,
            AvatarEntity avatar,
            BehaviourTreeConfig behaviourTreeConfig)
            : base(novatarEntity, novatarStateModel)
        {
            _avatar = avatar;
            _behaviourTreeConfig = behaviourTreeConfig;

            _behaviourTree = CreateTree();
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
                    .Do(nameof(EvaluateRelationshipOnTime), t => EvaluateRelationshipOnTime())
                .End()
                .Build();
        }

        private BehaviourTreeStatus FollowAvatar()
        {
            NovatarEntity.MoveTowards(_avatar.Position);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus EvaluateRelationshipOnTouch()
        {
            RelationshipStatus nextStatus = GetWeightedRandomRelationshipStatus();

            NovatarStateModel.SetCurrentRelationshipStatus(nextStatus);
            return BehaviourTreeStatus.Success;
        }

        private RelationshipStatus GetWeightedRandomRelationshipStatus()
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

                randomNumber = randomNumber - switchChance.WeightedChance;
            }

            return NovatarStateModel.CurrentRelationshipStatus.Value;
        }


        // ToDo This assumes it could switch from any status, but logically it only ever happens from Unacquainted -> Neutral
        private BehaviourTreeStatus EvaluateRelationshipOnTime()
        {
            var currentRelationship = NovatarStateModel.CurrentRelationshipStatus.Value;
            var currentTimePassed = NovatarStateModel.TimePassedInCurrentStatusSeconds.Value;

            var timeoutSeconds = _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds;

            // 0 -> status does not change spontaneously
            if (timeoutSeconds <= 0 || currentTimePassed < timeoutSeconds)
            {
                return BehaviourTreeStatus.Success;
            }

            // Switch to NEUTRAL based on Dice Roll
            var switchChance = _behaviourTreeConfig.UnacquaintedConfig.TimeBasedSwitchChance;
            var diceRoll = UnityEngine.Random.Range(0f, 1f);

            if (diceRoll <= switchChance)
            {
                NovatarStateModel.SetCurrentRelationshipStatus(RelationshipStatus.Neutral);
            }

            // Reset Time, so we re-evaluate only when the next interval is over
            NovatarStateModel.SetTimePassedInCurrentStatusSeconds(0);

            return BehaviourTreeStatus.Success;
        }
    }
}
