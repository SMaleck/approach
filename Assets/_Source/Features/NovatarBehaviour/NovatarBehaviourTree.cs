using _Source.Entities.Novatar;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour.Behaviours;
using _Source.Features.NovatarBehaviour.Data;
using _Source.Features.NovatarBehaviour.Nodes;
using _Source.Features.NovatarBehaviour.Sensors;
using _Source.Util;
using FluentBehaviourTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour
{
    public class NovatarBehaviourTree : AbstractDisposable, IInitializable
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, MovementController, NovatarBehaviourTree> { }

        [Inject] private readonly RangeSensor.Factory _rangeSensorFactory;

        [Inject] private readonly FollowAvatarNode.Factory _followAvatarNodeFactory;
        [Inject] private readonly IdleTimeoutNode.Factory _idleTimeoutNodeFactory;
        [Inject] private readonly FirstTouchNode.Factory _firstTouchNodeFactory;
        [Inject] private readonly SwitchEntityStateNode.Factory _switchEntityStateNodeFactory;

        private readonly INovatar _novatarEntity;
        private readonly INovatarStateModel _novatarStateModel;
        private readonly MovementController _movementController;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;
        private readonly SpawningBehaviour.Factory _spawningBehaviourFactory;
        private readonly TelemetryBehaviour.Factory _telemetryBehaviourFactory;
        private readonly UnacquaintedBehaviour.Factory _unacquaintedBehaviourFactory;
        private readonly NeutralBehaviour.Factory _neutralBehaviourFactory;
        private readonly FriendBehaviour.Factory _friendBehaviourFactory;
        private readonly EnemyBehaviour.Factory _enemyBehaviourFactory;
        private readonly IPauseStateModel _pauseStateModel;

        private IBehaviourTreeNode _behaviourTree;

        public NovatarBehaviourTree(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            MovementController movementController,
            BehaviourTreeConfig behaviourTreeConfig,
            SpawningBehaviour.Factory spawningBehaviourFactory,
            TelemetryBehaviour.Factory telemetryBehaviourFactory,
            UnacquaintedBehaviour.Factory unacquaintedBehaviourFactory,
            NeutralBehaviour.Factory neutralBehaviourFactory,
            FriendBehaviour.Factory friendBehaviourFactory,
            EnemyBehaviour.Factory enemyBehaviourFactory,
            IPauseStateModel pauseStateModel)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _movementController = movementController;
            _behaviourTreeConfig = behaviourTreeConfig;
            _spawningBehaviourFactory = spawningBehaviourFactory;
            _telemetryBehaviourFactory = telemetryBehaviourFactory;
            _unacquaintedBehaviourFactory = unacquaintedBehaviourFactory;
            _neutralBehaviourFactory = neutralBehaviourFactory;
            _friendBehaviourFactory = friendBehaviourFactory;
            _enemyBehaviourFactory = enemyBehaviourFactory;
            _pauseStateModel = pauseStateModel;
        }

        public void Initialize()
        {
            _behaviourTree = CreateTree();

            Observable.EveryLateUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value && _novatarStateModel.IsAlive.Value)
                .Subscribe(_ => _behaviourTree.Tick(new TimeData(Time.deltaTime)))
                .AddTo(Disposer);
        }

        private IBehaviourTreeNode CreateTree()
        {
            var spawningBehaviour = _spawningBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel,
                    _movementController)
                .Build();

            var telemetryBehaviour = _telemetryBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel)
                .Build();

            var unacquaintedBehaviour = _unacquaintedBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel,
                    _movementController)
                .Build();

            var neutralBehaviour = _neutralBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel,
                    _movementController)
                .Build();

            var friendBehaviour = _friendBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel,
                    _movementController)
                .Build();

            var enemyBehaviour = _enemyBehaviourFactory
                .Create(
                    _novatarEntity,
                    _novatarStateModel,
                    _movementController)
                .Build();

            var rangeSensor = _rangeSensorFactory.Create(_novatarEntity);

            var unacquaintedIdleTimeOutNode = _idleTimeoutNodeFactory.Create(
                    _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds);

            var followNode = _followAvatarNodeFactory.Create(
                rangeSensor,
                _movementController);

            var unacquaintedFirstTouchNode = _firstTouchNodeFactory.Create(
                _novatarEntity,
                _novatarStateModel,
                rangeSensor);

            var toNeutralStateNode = _switchEntityStateNodeFactory.Create(
                _novatarEntity,
                EntityState.Neutral);

            Observable.EveryLateUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value && _novatarStateModel.IsAlive.Value)
                .Subscribe(_ => rangeSensor.UpdateSensorReadings())
                .AddTo(Disposer);

            return new BehaviourTreeBuilder()
                .Parallel(nameof(NovatarBehaviourTree), 100, 100)
                    .Splice(telemetryBehaviour)
                    .Selector("RelationshipTreeSelector")
                        .Sequence(nameof(spawningBehaviour))
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Spawning))
                            .Splice(spawningBehaviour)
                            .End()
                        .Sequence(nameof(unacquaintedBehaviour))
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Unacquainted))
                            .Selector("")
                                .Do("", followNode.Tick)
                                .Do("", unacquaintedFirstTouchNode.Tick)
                                .Sequence("")
                                    .Do("", unacquaintedIdleTimeOutNode.Tick)
                                    .Do("", toNeutralStateNode.Tick)
                                .End()
                            .End()
                        .Sequence(nameof(neutralBehaviour))
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Neutral))
                            .Splice(neutralBehaviour)
                            .End()
                        .Sequence(nameof(friendBehaviour))
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Friend))
                            .Splice(friendBehaviour)
                            .End()
                        .Sequence(nameof(enemyBehaviour))
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Enemy))
                            .Splice(enemyBehaviour)
                            .End()
                    .End()
                .End()
                .Build();
        }

        private bool IsEntityState(EntityState status)
        {
            return _novatarStateModel.CurrentEntityState.Value == status;
        }
    }
}
