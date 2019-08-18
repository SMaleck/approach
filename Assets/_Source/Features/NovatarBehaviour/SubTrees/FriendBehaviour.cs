using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour.Data;
using FluentBehaviourTree;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public class FriendBehaviour : AbstractBehaviour
    {
        private readonly AvatarEntity _avatar;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private readonly IBehaviourTreeNode _behaviourTree;
        private RelationshipStatus _lastTrackedRelationShipStatus;

        public FriendBehaviour(
            NovatarEntity novatarEntity,
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
                .Selector("FriendTree")
                    .Sequence("FollowAvatar")
                        .Condition(nameof(IsInFollowRange), t => IsInFollowRange())
                        .Do(nameof(FollowAvatar), t => FollowAvatar())
                        .End()
                    .Sequence("FallingBehind")
                        .Do(nameof(TrackTimePassedSinceFallingBehind), TrackTimePassedSinceFallingBehind)
                        .Do(nameof(EvaluateRelationshipOnFallingBehind), t => EvaluateRelationshipOnFallingBehind())
                        .End()
                .End()
                .Build();
        }

        private BehaviourTreeStatus FollowAvatar()
        {
            NovatarStateModel.SetTimePassedSinceFallingBehindSeconds(0);
            NovatarEntity.MoveTowards(_avatar.Position);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus TrackTimePassedSinceFallingBehind(TimeData t)
        {
            var hasFallenBehind = !IsInTouchRange() && !IsInFollowRange();
            if (!hasFallenBehind)
            {
                NovatarStateModel.SetTimePassedSinceFallingBehindSeconds(0);
            }
            else
            {
                var currentTimePassed = NovatarStateModel.TimePassedSinceFallingBehindSeconds.Value;
                NovatarStateModel.SetTimePassedSinceFallingBehindSeconds(currentTimePassed + t.deltaTime);
            }

            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus EvaluateRelationshipOnFallingBehind()
        {
            var secondsSinceFallingBehind = NovatarStateModel.TimePassedSinceFallingBehindSeconds.Value;
            if (secondsSinceFallingBehind >= _behaviourTreeConfig.MaxSecondsToFallBehind)
            {
                NovatarStateModel.SetCurrentRelationshipStatus(RelationshipStatus.Neutral);
            }

            return BehaviourTreeStatus.Success;
        }
    }
}
