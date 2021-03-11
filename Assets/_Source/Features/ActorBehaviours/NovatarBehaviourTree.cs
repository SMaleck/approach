using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Data;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Util;
using BehaviourTreeSystem;
using System.Collections.Generic;
using System.Linq;
using _Source.Features.ActorSensors;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorBehaviours
{
    // ToDo V1 Move around on the playing field, increase leave time
    // ToDo V1 Sometimes move towards player
    // ToDo V1 FRIENDS: Absorb enemies
    // ToDo V1 FRIENDS: Leave when health is low
    public class NovatarBehaviourTree : AbstractDisposable, IInitializable
    {
        public class Factory : PlaceholderFactory<IActorStateModel, ISensorySystem, MovementController, NovatarBehaviourTree> { }

        [Inject] private readonly NodeGenerator.Factory _nodeGeneratorFactory;

        private readonly IActorStateModel _actorStateModel;
        private readonly ISensorySystem _sensorySystem;
        private readonly MovementController _movementController;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;
        private readonly IPauseStateModel _pauseStateModel;

        private IBehaviourTreeNode _behaviourTree;
        private List<IResettableNode> _resettableNodes;
        private List<IResettableNode> _resettableTimeoutNodes;

        public NovatarBehaviourTree(
            IActorStateModel actorStateModel,
            ISensorySystem sensorySystem,
            MovementController movementController,
            BehaviourTreeConfig behaviourTreeConfig,
            IPauseStateModel pauseStateModel)
        {
            _actorStateModel = actorStateModel;
            _sensorySystem = sensorySystem;
            _movementController = movementController;
            _behaviourTreeConfig = behaviourTreeConfig;
            _pauseStateModel = pauseStateModel;
        }

        public void Initialize()
        {
            _behaviourTree = CreateTree();

            var healthDataComponent = _actorStateModel.Get<HealthDataComponent>();

            Observable.EveryLateUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value && healthDataComponent.IsAlive.Value)
                .Subscribe(_ => _behaviourTree.Tick(new TimeData(Time.deltaTime)))
                .AddTo(Disposer);

            _actorStateModel.OnReset
                .Subscribe(_ => ResetNodes())
                .AddTo(Disposer);

            _actorStateModel.OnResetIdleTimeouts
                .Subscribe(_ => ResetTimeoutNodes())
                .AddTo(Disposer);
        }

        private IBehaviourTreeNode CreateTree()
        {
            var nodeGenerator = _nodeGeneratorFactory.Create(
                _actorStateModel,
                _sensorySystem,
                _movementController);

            var unacquaintedRandomTimeoutNode = nodeGenerator.CreateIdleTimeoutRandomNode(
                _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds,
                _behaviourTreeConfig.UnacquaintedConfig.TimeBasedSwitchChance);

            var unacquaintedFirstTouchNode = nodeGenerator.CreateFirstTouchNode();

            var toNeutralStateNode = nodeGenerator.CreateSwitchEntityStateNode(
                EntityState.Neutral);

            var toUnacquaintedStateNode = nodeGenerator.CreateSwitchEntityStateNode(
                EntityState.Unacquainted);

            var friendTimeoutNode = nodeGenerator.CreateIdleTimeoutNode(
                _behaviourTreeConfig.MaxSecondsToFallBehind);

            var enemyTimeoutNode = nodeGenerator.CreateIdleTimeoutNode(
                _behaviourTreeConfig.EnemyLeavingTimeoutSeconds);

            var followAvatarNode = nodeGenerator.CreateFollowAvatarNode();
            var deactivateSelfNode = nodeGenerator.CreateDeactivateSelfNode();
            var leaveScreenNode = nodeGenerator.CreateLeaveScreenNode();
            var damageAvatarNode = nodeGenerator.CreateDamageAvatarNode();
            var lightSwitchNode = nodeGenerator.CreateLightSwitchNode();
            var enterScreenNode = nodeGenerator.CreateEnterScreenNode();
            var movementNode = nodeGenerator.CreateMovementNode();

            // @formatter:off
            var tree = new BehaviourTreeBuilder()
                .Parallel(100, 100)
                    .Selector()
                        .Sequence()
                            .Condition(t => IsEntityState(EntityState.Spawning))
                            .Sequence()
                                .Do(lightSwitchNode.Tick)
                                .Do(enterScreenNode.Tick)
                                .Do(toUnacquaintedStateNode.Tick)
                                .End()
                            .End()
                        .Sequence()
                            .Condition(t => IsEntityState(EntityState.Unacquainted))
                            .Selector()
                                .Sequence()
                                    .Do(followAvatarNode.Tick) // TODO Problem
                                    .Do(movementNode.Tick)
                                    .Do(unacquaintedFirstTouchNode.Tick)
                                    .End()
                                .Sequence()
                                    .Do(unacquaintedRandomTimeoutNode.Tick)
                                    .Do(toNeutralStateNode.Tick)
                                    .End()
                                .End()
                            .End()
                        .Sequence()
                            .Condition(t => IsEntityState(EntityState.Neutral))
                            .Sequence()
                                .Do(leaveScreenNode.Tick)
                                .Do(deactivateSelfNode.Tick)
                                .End()
                            .End()
                        .Sequence()
                            .Condition(t => IsEntityState(EntityState.Friend))
                            .Selector()
                                .Sequence()
                                    .Do(followAvatarNode.Tick)
                                    .Do(movementNode.Tick)
                                    .End()
                                .Sequence()
                                    .Do(friendTimeoutNode.Tick)
                                    .Do(toNeutralStateNode.Tick)
                                    .End()
                                .End()
                            .End()
                        .Sequence()
                            .Condition(t => IsEntityState(EntityState.Enemy))
                            .Selector()
                                .Sequence()
                                    .Do(damageAvatarNode.Tick)
                                    .Do(enemyTimeoutNode.Tick)
                                    .Do(toNeutralStateNode.Tick)
                                    .End()
                                .Sequence()
                                    .Do(followAvatarNode.Tick)
                                    .Do(movementNode.Tick)
                                    .End()
                                .End()
                            .End()
                    .End()
                .End()
                .Build();
            // @formatter:on

            AggregateNodes(nodeGenerator);

            return tree;
        }

        private void AggregateNodes(NodeGenerator nodeGenerator)
        {
            _resettableNodes = nodeGenerator.GeneratedNodes
                .OfType<IResettableNode>()
                .ToList();

            var timeoutNodes = nodeGenerator.GeneratedNodes
                .OfType<IdleTimeoutNode>()
                .Cast<IResettableNode>();

            var timeoutRandomNodes = nodeGenerator.GeneratedNodes
                .OfType<IdleTimeoutRandomNode>()
                .Cast<IResettableNode>();

            _resettableTimeoutNodes = new List<IResettableNode>();
            _resettableTimeoutNodes.AddRange(timeoutNodes);
            _resettableTimeoutNodes.AddRange(timeoutRandomNodes);
        }

        private bool IsEntityState(EntityState status)
        {
            var _relationshipDataComponent = _actorStateModel.Get<RelationshipDataComponent>();
            return _relationshipDataComponent.Relationship.Value == status;
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
