using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.GameWorld;
using _Source.Util;
using FluentBehaviourTree;
using System;
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
            var telemetryTree = CreateTelemetryTree();
            var unacquaintedTree = CreateUnacquaintedTree();
            var neutralTree = CreateNeutralTree();

            return new BehaviourTreeBuilder()
                .Parallel("Tree", 20, 20)
                    .Splice(telemetryTree)
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

        private IBehaviourTreeNode CreateTelemetryTree()
        {
            return new BehaviourTreeBuilder()
                .Parallel("TelemetryTree", 20, 20)
                    .Do(nameof(TrackTimePassedInCurrentStatus), TrackTimePassedInCurrentStatus)
                    .Do(nameof(CalculateDistanceToAvatar), t => CalculateDistanceToAvatar())
                    .Do(nameof(EvaluateRelationshipOnTime), t => EvaluateRelationshipOnTime())
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
                        .Do(nameof(EvaluateRelationshipOnTouch), t => EvaluateRelationshipOnTouch())
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

        private BehaviourTreeStatus TrackTimePassedInCurrentStatus(TimeData t)
        {
            var currentTimePassed = _novatarStateModel.TimePassedInCurrentStatusSeconds.Value;
            _novatarStateModel.SetTimePassedInCurrentStatusSeconds(currentTimePassed + t.deltaTime);

            return BehaviourTreeStatus.Success;
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

        private BehaviourTreeStatus EvaluateRelationshipOnTouch()
        {
            var currentRelationship = _novatarStateModel.CurrentRelationshipStatus.Value;
            RelationshipStatus nextStatus = currentRelationship;

            switch (currentRelationship)
            {
                case RelationshipStatus.Unacquainted:
                    nextStatus = RelationshipStatus.Neutral;
                    break;

                case RelationshipStatus.Enemy:
                    break;

                case RelationshipStatus.Friend:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _novatarStateModel.SetCurrentRelationshipStatus(nextStatus);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus EvaluateRelationshipOnTime()
        {
            var currentRelationship = _novatarStateModel.CurrentRelationshipStatus.Value;
            var currentTimePassed = _novatarStateModel.TimePassedInCurrentStatusSeconds.Value;

            var timeoutSeconds = _novatarConfig.GetRelationshipTimeout(currentRelationship);

            // 0 -> status does not change spontaneously
            if (timeoutSeconds <= 0 || currentTimePassed < timeoutSeconds)
            {
                return BehaviourTreeStatus.Success;
            }

            // Switch to NEUTRAL based on Dice Roll
            var switchChance = _novatarConfig.GetRelationshipSwitchChance(currentRelationship);
            var diceRoll = UnityEngine.Random.Range(0f, 1f);

            if (diceRoll <= switchChance)
            {
                _novatarStateModel.SetCurrentRelationshipStatus(RelationshipStatus.Neutral);
            }

            return BehaviourTreeStatus.Success;
        }
    }
}
