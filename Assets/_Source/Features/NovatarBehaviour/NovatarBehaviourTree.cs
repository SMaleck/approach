using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.GameWorld;
using _Source.Util;
using FluentBehaviourTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour
{
    public class NovatarBehaviourTree : AbstractDisposable, IInitializable
    {
        public class Factory : PlaceholderFactory<NovatarEntity, NovatarStateModel, NovatarBehaviourTree> { }

        private readonly NovatarEntity _novatar;
        private readonly NovatarStateModel _novatarStateModel;
        private readonly NovatarConfig _novatarConfig;
        private readonly AvatarEntity _avatar;
        private readonly ScreenSizeController _screenSizeController;

        private IBehaviourTreeNode _behaviourTree;

        public NovatarBehaviourTree(
            NovatarEntity novatar,
            NovatarStateModel novatarStateModel,
            NovatarConfig novatarConfig,
            AvatarEntity avatar,
            ScreenSizeController screenSizeController)
        {
            _novatar = novatar;
            _novatarStateModel = novatarStateModel;
            _novatarConfig = novatarConfig;
            _avatar = avatar;
            _screenSizeController = screenSizeController;
        }

        public void Initialize()
        {
            _behaviourTree = CreateTree();

            Observable.EveryLateUpdate()
                .Where(_ => _novatar.IsActive)
                .Subscribe(_ => _behaviourTree.Tick(new TimeData(Time.deltaTime)))
                .AddTo(Disposer);
        }

        private IBehaviourTreeNode CreateTree()
        {
            var unacquaintedTree = CreateUnacquaintedTree();
            var neutralTree = CreateNeutralTree();

            return new BehaviourTreeBuilder()
                .Parallel("Tree", 20, 20)
                    .Do(nameof(CalculateDistanceToAvatar), t => CalculateDistanceToAvatar())
                    .Selector("RelationshipTreeSelector")
                        .Sequence("UnacquaintedSequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Unacquainted))
                            .Splice(unacquaintedTree)
                            .End()
                        .Sequence("UnacquaintedSequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Neutral))
                            .Splice(neutralTree)
                            .End()
                    .End()
                .End()
                .Build();
        }

        private IBehaviourTreeNode CreateUnacquaintedTree()
        {
            return new BehaviourTreeBuilder()
                .Selector("UnacquaintedTree")
                    .Sequence("FollowAvatar")
                        .Condition(nameof(IsInFollowRange), t => IsInFollowRange())
                        .Do(nameof(FollowAvatar), t => FollowAvatar())
                        .End()
                    .Sequence("TouchAvatar")
                        .Condition(nameof(IsInTouchRange), t => IsInTouchRange())
                        .Do(nameof(EvaluateRelationship), t => EvaluateRelationship())
                        .End()
                .End()
                .Build();
        }

        private IBehaviourTreeNode CreateNeutralTree()
        {
            return new BehaviourTreeBuilder()
                .Selector("NeutralTree")
                    .Sequence("LeavePlayingField")
                        .Condition(nameof(IsWithinScreenBounds), t => IsWithinScreenBounds())
                        .Do(nameof(MoveToSpawnPosition), t => MoveToSpawnPosition())
                        .End()
                    .Sequence("Deactivate")
                        .Condition(nameof(IsWithinScreenBounds), t => !IsWithinScreenBounds())
                        .Do(nameof(DeactivateSelf), t => DeactivateSelf())
                        .End()
                .End()
                .Build();
        }

        private bool IsCurrentRelationshipStatus(RelationshipStatus status)
        {
            return _novatarStateModel.CurrentRelationshipStatus.Value == status;
        }

        private BehaviourTreeStatus CalculateDistanceToAvatar()
        {
            var sqrDistance = _novatar.GetSquaredDistanceTo(_avatar);
            _novatarStateModel.SetCurrentDistanceToAvatar(sqrDistance);

            return BehaviourTreeStatus.Success;
        }

        private bool IsInFollowRange()
        {
            var isInRange = _novatarStateModel.CurrentDistanceToAvatar.Value <= _novatar.SqrRange;
            return isInRange && !IsInTouchRange();
        }

        private bool IsInTouchRange()
        {
            return _novatarStateModel.CurrentDistanceToAvatar.Value <= _novatar.SqrTargetReachedThreshold;
        }

        private bool IsWithinScreenBounds()
        {
            return !_screenSizeController.IsOutOfScreenBounds(
                _novatar.Position,
                _novatar.Size);
        }

        private BehaviourTreeStatus FollowAvatar()
        {
            _novatar.MoveTowards(_avatar.Position);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus MoveToSpawnPosition()
        {
            _novatar.MoveTowards(_novatar.SpawnedAtPosition);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus DeactivateSelf()
        {
            _novatarStateModel.SetIsAlive(false);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus EvaluateRelationship()
        {
            _novatarStateModel.SetCurrentRelationshipStatus(RelationshipStatus.Neutral);
            return BehaviourTreeStatus.Success;
        }
    }
}
