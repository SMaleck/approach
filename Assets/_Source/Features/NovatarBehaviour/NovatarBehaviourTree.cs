using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour.Behaviours;
using _Source.Util;
using FluentBehaviourTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour
{
    public class NovatarBehaviourTree : AbstractDisposable, IInitializable
    {
        public class Factory : PlaceholderFactory<INovatar, NovatarStateModel, NovatarBehaviourTree> { }

        private readonly INovatar _novatarEntity;
        private readonly NovatarStateModel _novatarStateModel;
        private readonly SpawningBehaviour.Factory _spawningBehaviourFactory;
        private readonly TelemetryBehaviour.Factory _telemetryBehaviourFactory;
        private readonly UnacquaintedBehaviour.Factory _unacquaintedBehaviourFactory;
        private readonly NeutralBehaviour.Factory _neutralBehaviourFactory;
        private readonly FriendBehaviour.Factory _friendBehaviourFactory;
        private readonly EnemyBehaviour.Factory _enemyBehaviourFactory;

        private IBehaviourTreeNode _behaviourTree;

        public NovatarBehaviourTree(
            INovatar novatarEntity,
            NovatarStateModel novatarStateModel,
            SpawningBehaviour.Factory spawningBehaviourFactory,
            TelemetryBehaviour.Factory telemetryBehaviourFactory,
            UnacquaintedBehaviour.Factory unacquaintedBehaviourFactory,
            NeutralBehaviour.Factory neutralBehaviourFactory,
            FriendBehaviour.Factory friendBehaviourFactory,
            EnemyBehaviour.Factory enemyBehaviourFactory)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _spawningBehaviourFactory = spawningBehaviourFactory;
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
                .Where(_ => _novatarStateModel.IsAlive.Value)
                .Subscribe(_ => _behaviourTree.Tick(new TimeData(Time.deltaTime)))
                .AddTo(Disposer);
        }

        private IBehaviourTreeNode CreateTree()
        {
            var spawningBehaviour = _spawningBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var telemetryBehaviour = _telemetryBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var unacquaintedBehaviour = _unacquaintedBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var neutralBehaviour = _neutralBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var friendBehaviour = _friendBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var enemyBehaviour = _enemyBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            return new BehaviourTreeBuilder()
                .Parallel(nameof(NovatarBehaviourTree), 20, 20)
                    .Splice(telemetryBehaviour)
                    .Selector("RelationshipTreeSelector")
                        .Sequence("SpawningBehaviourSequence")
                            .Condition(nameof(IsRelationshipStatus), t => IsRelationshipStatus(RelationshipStatus.Spawning))
                            .Splice(spawningBehaviour)
                            .End()
                        .Sequence("UnacquaintedBehaviourSequence")
                            .Condition(nameof(IsRelationshipStatus), t => IsRelationshipStatus(RelationshipStatus.Unacquainted))
                            .Splice(unacquaintedBehaviour)
                            .End()
                        .Sequence("NeutralBehaviourSequence")
                            .Condition(nameof(IsRelationshipStatus), t => IsRelationshipStatus(RelationshipStatus.Neutral))
                            .Splice(neutralBehaviour)
                            .End()
                        .Sequence("FriendBehaviourSequence")
                            .Condition(nameof(IsRelationshipStatus), t => IsRelationshipStatus(RelationshipStatus.Friend))
                            .Splice(friendBehaviour)
                            .End()
                        .Sequence("EnemyBehaviourSequence")
                            .Condition(nameof(IsRelationshipStatus), t => IsRelationshipStatus(RelationshipStatus.Enemy))
                            .Splice(enemyBehaviour)
                            .End()
                    .End()
                .End()
                .Build();
        }

        private bool IsRelationshipStatus(RelationshipStatus status)
        {
            return _novatarStateModel.CurrentRelationshipStatus.Value == status;
        }
    }
}
