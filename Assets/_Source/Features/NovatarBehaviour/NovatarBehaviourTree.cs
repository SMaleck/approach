using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.AvatarState;
using _Source.Features.GameWorld;
using _Source.Features.NovatarBehaviour.Data;
using _Source.Features.NovatarBehaviour.SubTrees;
using _Source.Util;
using FluentBehaviourTree;
using System.Linq;
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
        private readonly BehaviourTreeConfig _behaviourTreeConfig;
        private readonly AvatarEntity _avatar;
        private readonly ScreenSizeController _screenSizeController;
        private readonly IDamageReceiver _avatarDamageReceiver;

        private IBehaviourTreeNode _behaviourTree;

        public NovatarBehaviourTree(
            NovatarEntity novatar,
            NovatarStateModel novatarStateModel,
            NovatarConfig novatarConfig,
            BehaviourTreeConfig behaviourTreeConfig,
            AvatarEntity avatar,
            ScreenSizeController screenSizeController,
            IDamageReceiver avatarDamageReceiver)
        {
            _novatar = novatar;
            _novatarStateModel = novatarStateModel;
            _novatarConfig = novatarConfig;
            _behaviourTreeConfig = behaviourTreeConfig;
            _avatar = avatar;
            _screenSizeController = screenSizeController;
            _avatarDamageReceiver = avatarDamageReceiver;
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
            var friendTree = CreateFriendTree();
            var enemyTree = CreateEnemyTree();

            var telemetrySubTree = new TelemetryBehaviour(_novatar, _novatarStateModel, _avatar);

            return new BehaviourTreeBuilder()
                .Parallel("Tree", 20, 20)
                    .Splice(telemetrySubTree.GetTree())
                    .Do(nameof(EvaluateRelationshipOnTime), t => EvaluateRelationshipOnTime())
                    .Selector("RelationshipTreeSelector")
                        .Sequence("UnacquaintedSequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Unacquainted))
                            .Splice(unacquaintedTree)
                            .End()
                        .Sequence("NeutralSequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Neutral))
                            .Splice(neutralTree)
                            .End()
                        .Sequence("FriendSequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Friend))
                            .Splice(friendTree)
                            .End()
                        .Sequence("EnemySequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Enemy))
                            .Splice(enemyTree)
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

        private IBehaviourTreeNode CreateFriendTree()
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

        private IBehaviourTreeNode CreateEnemyTree()
        {
            return new BehaviourTreeBuilder()
                .Selector("EnemyTree")
                    .Sequence("DamageAvatar")
                        .Condition(nameof(IsInTouchRange), t => IsInTouchRange())
                        .Do(nameof(DamageAvatar), t => DamageAvatar())
                        .End()
                .End()
                .Build();
        }


        private BehaviourTreeStatus TrackTimePassedSinceFallingBehind(TimeData t)
        {
            var currentTimePassed = _novatarStateModel.TimePassedSinceFallingBehindSeconds.Value;
            _novatarStateModel.SetTimePassedSinceFallingBehindSeconds(currentTimePassed + t.deltaTime);

            return BehaviourTreeStatus.Success;
        }

        private bool IsCurrentRelationshipStatus(RelationshipStatus status)
        {
            return _novatarStateModel.CurrentRelationshipStatus.Value == status;
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
            _novatarStateModel.SetTimePassedSinceFallingBehindSeconds(0);
            _novatar.MoveTowards(_avatar.Position);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus MoveToSpawnPosition()
        {
            var spawnPosition = _novatarStateModel.SpawnPosition.Value;
            _novatar.MoveTowards(spawnPosition);
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
                    nextStatus = GetWeightedRandomRelationshipStatus();
                    break;

                case RelationshipStatus.Enemy:
                    break;

                // Some relationships don't change on touch
                case RelationshipStatus.Neutral:
                case RelationshipStatus.Friend:
                    break;
            }

            _novatarStateModel.SetCurrentRelationshipStatus(nextStatus);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus EvaluateRelationshipOnTime()
        {
            var currentRelationship = _novatarStateModel.CurrentRelationshipStatus.Value;
            var currentTimePassed = _novatarStateModel.TimePassedInCurrentStatusSeconds.Value;

            var timeoutSeconds = _behaviourTreeConfig.GetEvaluationTimeoutSeconds(currentRelationship);

            // 0 -> status does not change spontaneously
            if (timeoutSeconds <= 0 || currentTimePassed < timeoutSeconds)
            {
                return BehaviourTreeStatus.Success;
            }

            // Switch to NEUTRAL based on Dice Roll
            var switchChance = _behaviourTreeConfig.GetTimeBasedSwitchChance(currentRelationship);
            var diceRoll = UnityEngine.Random.Range(0f, 1f);

            if (diceRoll <= switchChance)
            {
                _novatarStateModel.SetCurrentRelationshipStatus(RelationshipStatus.Neutral);
            }

            // Reset Time, so we re-evaluate only when the next interval is over
            _novatarStateModel.SetTimePassedInCurrentStatusSeconds(0);

            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus EvaluateRelationshipOnFallingBehind()
        {
            var secondsSinceFallingBehind = _novatarStateModel.TimePassedSinceFallingBehindSeconds.Value;
            if (secondsSinceFallingBehind >= _behaviourTreeConfig.MaxSecondsToFallBehind)
            {
                _novatarStateModel.SetCurrentRelationshipStatus(RelationshipStatus.Neutral);
            }

            return BehaviourTreeStatus.Success;
        }

        private RelationshipStatus GetWeightedRandomRelationshipStatus()
        {
            var currentRelationship = _novatarStateModel.CurrentRelationshipStatus.Value;
            var switchChances = _behaviourTreeConfig.GetRelationshipSwitchWeights(currentRelationship);

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

            return currentRelationship;
        }

        private BehaviourTreeStatus DamageAvatar()
        {
            var damage = _novatarConfig.TouchDamage;
            _avatarDamageReceiver.ReceiveDamage(damage);
            DeactivateSelf();

            return BehaviourTreeStatus.Success;
        }
    }
}
