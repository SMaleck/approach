using _Source.Entities.Novatar;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour.Behaviours;
using _Source.Features.NovatarBehaviour.Data;
using _Source.Features.NovatarBehaviour.Nodes;
using _Source.Features.NovatarBehaviour.Sensors;
using _Source.Util;
using FluentBehaviourTree;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour
{
    public class NovatarBehaviourTree : AbstractDisposable, IInitializable
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, ISensorySystem, MovementController, NovatarBehaviourTree> { }

        [Inject] private readonly NodeGenerator.Factory _nodeGeneratorFactory;

        [Inject] private readonly FollowAvatarNode.Factory _followAvatarNodeFactory;
        [Inject] private readonly IdleTimeoutNode.Factory _idleTimeoutNodeFactory;
        [Inject] private readonly FirstTouchNode.Factory _firstTouchNodeFactory;
        [Inject] private readonly SwitchEntityStateNode.Factory _switchEntityStateNodeFactory;

        [Inject] private readonly SpawningBehaviour.Factory _spawningBehaviourFactory;
        [Inject] private readonly TelemetryBehaviour.Factory _telemetryBehaviourFactory;
        [Inject] private readonly NeutralBehaviour.Factory _neutralBehaviourFactory;
        [Inject] private readonly FriendBehaviour.Factory _friendBehaviourFactory;
        [Inject] private readonly EnemyBehaviour.Factory _enemyBehaviourFactory;

        private readonly INovatar _novatarEntity;
        private readonly INovatarStateModel _novatarStateModel;
        private readonly ISensorySystem _sensorySystem;
        private readonly MovementController _movementController;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;
        private readonly IPauseStateModel _pauseStateModel;

        private IBehaviourTreeNode _behaviourTree;
        private List<IResettableNode> _resettableNodes;
        private List<IResettableNode> _resettableTimeoutNodes;

        public NovatarBehaviourTree(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            ISensorySystem sensorySystem,
            MovementController movementController,
            BehaviourTreeConfig behaviourTreeConfig,
            IPauseStateModel pauseStateModel)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _sensorySystem = sensorySystem;
            _movementController = movementController;
            _behaviourTreeConfig = behaviourTreeConfig;
            _pauseStateModel = pauseStateModel;
        }

        public void Initialize()
        {
            _behaviourTree = CreateTree();

            Observable.EveryLateUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value && _novatarStateModel.IsAlive.Value)
                .Subscribe(_ => _behaviourTree.Tick(new TimeData(Time.deltaTime)))
                .AddTo(Disposer);

            _novatarStateModel.OnReset
                .Subscribe(_ => ResetNodes())
                .AddTo(Disposer);

            _novatarStateModel.OnResetIdleTimeouts
                .Subscribe(_ => ResetTimeoutNodes())
                .AddTo(Disposer);
        }

        private IBehaviourTreeNode CreateTree()
        {
            var nodeGenerator = _nodeGeneratorFactory.Create();
            nodeGenerator.SetupForNovatar(
                _novatarEntity,
                _novatarStateModel,
                _sensorySystem,
                _movementController);

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

            var unacquaintedRandomTimeoutNode = nodeGenerator.CreateIdleTimeoutRandomNode(
                _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds,
                _behaviourTreeConfig.UnacquaintedConfig.TimeBasedSwitchChance);

            var unacquaintedFirstTouchNode = nodeGenerator.CreateFirstTouchNode();

            var followNode = nodeGenerator.CreateFollowAvatarNode();

            var toNeutralStateNode = nodeGenerator.CreateSwitchEntityStateNode(
                EntityState.Neutral);

            var tree = new BehaviourTreeBuilder()
                .Parallel(nameof(NovatarBehaviourTree), 100, 100)
                    .Splice(telemetryBehaviour)
                    .Selector("RelationshipTreeSelector")
                        .Sequence(EntityState.Spawning.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Spawning))
                            .Splice(spawningBehaviour)
                            .End()
                        .Sequence(EntityState.Unacquainted.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Unacquainted))
                            .Selector("Selector")
                                .Do(nameof(FollowAvatarNode), followNode.Tick)
                                .Do(nameof(FirstTouchNode), unacquaintedFirstTouchNode.Tick)
                                .Sequence("Sequence")
                                    .Do(nameof(IdleTimeoutRandomNode), unacquaintedRandomTimeoutNode.Tick)
                                    .Do(nameof(SwitchEntityStateNode), toNeutralStateNode.Tick)
                                .End()
                            .End().End()
                        .Sequence(EntityState.Neutral.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Neutral))
                            .Splice(neutralBehaviour)
                            .End()
                        .Sequence(EntityState.Friend.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Friend))
                            .Splice(friendBehaviour)
                            .End()
                        .Sequence(EntityState.Enemy.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Enemy))
                            .Splice(enemyBehaviour)
                            .End()
                    .End()
                .End()
                .Build();

            _resettableNodes = nodeGenerator.GeneratedNodes
                .OfType<IResettableNode>()
                .ToList();

            _resettableTimeoutNodes = nodeGenerator.GeneratedNodes
                .OfType<IdleTimeoutNode>()
                .Cast<IResettableNode>()
                .ToList();

            return tree;
        }

        private bool IsEntityState(EntityState status)
        {
            return _novatarStateModel.CurrentEntityState.Value == status;
        }

        private void ResetNodes()
        {
            _resettableNodes.ForEach(node => node.Reset());
        }

        private void ResetTimeoutNodes()
        {
            _resettableTimeoutNodes.ForEach(node => node.Reset());
        }
    }
}
