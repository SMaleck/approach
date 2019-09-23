using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour.Data;
using FluentBehaviourTree;
using UniRx;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Behaviours
{
    public class FriendBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, MovementController, FriendBehaviour> { }

        private readonly MovementController _movementController;
        private readonly IAvatar _avatar;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private readonly IBehaviourTreeNode _behaviourTree;

        private EntityState _lastTrackedRelationShipStatus;
        private double _timeSinceFallingBehindSeconds = 0;
        
        public FriendBehaviour(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            MovementController movementController,
            IAvatar avatar,
            BehaviourTreeConfig behaviourTreeConfig)
            : base(novatarEntity, novatarStateModel)
        {
            _movementController = movementController;
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

            if (IsInTouchRange())
            {
                _movementController.Stop();

                return BehaviourTreeStatus.Success;
            }
            
            _movementController.MoveToTarget(_avatar.Position);
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
                NovatarEntity.SwitchToEntityState(EntityState.Neutral);
            }

            return BehaviourTreeStatus.Success;
        }
    }
}
