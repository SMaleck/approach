using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour.Data;
using FluentBehaviourTree;
using UniRx;
using Zenject;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public class FriendBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<NovatarEntity, NovatarStateModel, FriendBehaviour> { }

        private readonly AvatarEntity _avatar;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private readonly IBehaviourTreeNode _behaviourTree;

        private RelationshipStatus _lastTrackedRelationShipStatus;
        private double _timeSinceFallingBehindSeconds = 0;
        
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

            NovatarStateModel.OnReset
                .Subscribe(_ => Reset())
                .AddTo(Disposer);
        }

        private void Reset()
        {
            _timeSinceFallingBehindSeconds = 0;
        }

        public override IBehaviourTreeNode Build()
        {
            return _behaviourTree;
        }

        private IBehaviourTreeNode CreateTree()
        {
            return new BehaviourTreeBuilder()
                .Selector(nameof(FriendBehaviour))
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
            _timeSinceFallingBehindSeconds = 0;
            NovatarEntity.MoveTowards(_avatar.Position);

            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus TrackTimePassedSinceFallingBehind(TimeData t)
        {
            var hasFallenBehind = !IsInTouchRange() && !IsInFollowRange();
            if (!hasFallenBehind)
            {
                _timeSinceFallingBehindSeconds = 0;
            }
            else
            {
                _timeSinceFallingBehindSeconds += t.deltaTime;
            }

            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus EvaluateRelationshipOnFallingBehind()
        {
            if (_timeSinceFallingBehindSeconds >= _behaviourTreeConfig.MaxSecondsToFallBehind)
            {
                NovatarStateModel.SetCurrentRelationshipStatus(RelationshipStatus.Neutral);
            }

            return BehaviourTreeStatus.Success;
        }
    }
}
