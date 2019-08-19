using _Source.Entities.Novatar;
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
        private readonly TelemetryBehaviour.Factory _telemetryBehaviourFactory;
        private readonly UnacquaintedBehaviour.Factory _unacquaintedBehaviourFactory;
        private readonly NeutralBehaviour.Factory _neutralBehaviourFactory;
        private readonly FriendBehaviour.Factory _friendBehaviourFactory;
        private readonly EnemyBehaviour.Factory _enemyBehaviourFactory;

        private IBehaviourTreeNode _behaviourTree;

        public NovatarBehaviourTree(
            NovatarEntity novatarEntity,
            NovatarStateModel novatarStateModel,
            TelemetryBehaviour.Factory telemetryBehaviourFactory,
            UnacquaintedBehaviour.Factory unacquaintedBehaviourFactory,
            NeutralBehaviour.Factory neutralBehaviourFactory,
            FriendBehaviour.Factory friendBehaviourFactory,
            EnemyBehaviour.Factory enemyBehaviourFactory)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _telemetryBehaviourFactory = telemetryBehaviourFactory;
            _unacquaintedBehaviourFactory = unacquaintedBehaviourFactory;
            _neutralBehaviourFactory = neutralBehaviourFactory;
            _friendBehaviourFactory = friendBehaviourFactory;
            _enemyBehaviourFactory = enemyBehaviourFactory;
        }

        public void Initialize()
        {
            _behaviourTree = CreateTree();

            Observable.EveryLateUpdate()
                .Where(_ => _novatarEntity.IsActive)
                .Subscribe(_ => _behaviourTree.Tick(new TimeData(Time.deltaTime)))
                .AddTo(Disposer);
        }

        private IBehaviourTreeNode CreateTree()
        {
            var telemetrySubTree = _telemetryBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var unacquaintedSubTree = _unacquaintedBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var neutralSubTree = _neutralBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var friendSubTree = _friendBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var enemySubTree = _enemyBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
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

        private bool IsCurrentRelationshipStatus(RelationshipStatus status)
        {
            return _novatarStateModel.CurrentRelationshipStatus.Value == status;
        }
    }
}
