using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.AvatarState;
using _Source.Features.GameWorld;
using _Source.Features.NovatarBehaviour.Data;
using _Source.Features.NovatarBehaviour.SubTrees;
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

        private readonly NovatarEntity _novatarEntity;
        private readonly NovatarStateModel _novatarStateModel;
        private readonly NovatarConfig _novatarConfig;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;
        private readonly AvatarEntity _avatar;
        private readonly ScreenSizeController _screenSizeController;
        private readonly IDamageReceiver _avatarDamageReceiver;

        private IBehaviourTreeNode _behaviourTree;

        public NovatarBehaviourTree(
            NovatarEntity novatarEntity,
            NovatarStateModel novatarStateModel,
            NovatarConfig novatarConfig,
            BehaviourTreeConfig behaviourTreeConfig,
            AvatarEntity avatar,
            ScreenSizeController screenSizeController,
            IDamageReceiver avatarDamageReceiver)
        {
            _novatarEntity = novatarEntity;
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
                .Where(_ => _novatarEntity.IsActive)
                .Do(_ => LogNovatarStatus())
                .Subscribe(_ => _behaviourTree.Tick(new TimeData(Time.deltaTime)))
                .AddTo(Disposer);
        }

        private IBehaviourTreeNode CreateTree()
        {
            var telemetrySubTree = new TelemetryBehaviour(
                    _novatarEntity,
                    _novatarStateModel,
                    _avatar)
                .Build();

            var unacquaintedSubTree = new UnacquaintedBehaviour(
                    _novatarEntity,
                    _novatarStateModel,
                    _avatar,
                    _behaviourTreeConfig)
                .Build();

            var neutralSubTree = new NeutralBehaviour(
                    _novatarEntity,
                    _novatarStateModel,
                    _screenSizeController)
                .Build();

            var friendSubTree = new FriendBehaviour(
                    _novatarEntity,
                    _novatarStateModel,
                    _avatar,
                    _behaviourTreeConfig)
                .Build();

            var enemySubTree = new EnemyBehaviour(
                    _novatarEntity,
                    _novatarStateModel,
                    _novatarConfig,
                    _avatarDamageReceiver)
                .Build();

            return new BehaviourTreeBuilder()
                .Parallel(nameof(NovatarBehaviourTree), 20, 20)
                    .Splice(telemetrySubTree)
                    .Selector("RelationshipTreeSelector")
                        .Sequence("UnacquaintedSequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Unacquainted))
                            .Splice(unacquaintedSubTree)
                            .End()
                        .Sequence("NeutralSequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Neutral))
                            .Splice(neutralSubTree)
                            .End()
                        .Sequence("FriendSequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Friend))
                            .Splice(friendSubTree)
                            .End()
                        .Sequence("EnemySequence")
                            .Condition(nameof(IsCurrentRelationshipStatus), t => IsCurrentRelationshipStatus(RelationshipStatus.Enemy))
                            .Splice(enemySubTree)
                            .End()
                    .End()
                .End()
                .Build();
        }

        private void LogNovatarStatus()
        {
            App.Logger.Log($"{_novatarEntity.name} | ALIVE: {_novatarStateModel.IsAlive.Value} | STATUS: {_novatarStateModel.CurrentRelationshipStatus.Value}");
        }

        private bool IsCurrentRelationshipStatus(RelationshipStatus status)
        {
            return _novatarStateModel.CurrentRelationshipStatus.Value == status;
        }
    }
}
