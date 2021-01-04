using _Source.Entities.Novatar;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour.Data;
using _Source.Features.NovatarBehaviour.Nodes;
using _Source.Features.NovatarBehaviour.Sensors;
using _Source.Util;
using BehaviourTreeSystem;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour
{
    // ToDo V1 Move around on the playing field, increase leave time
    // ToDo V1 Sometimes move towards player
    // ToDo V1 FRIENDS: Absorb enemies
    // ToDo V1 FRIENDS: Leave when health is low
    public class NovatarBehaviourTree : AbstractDisposable, IInitializable
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, ISensorySystem, MovementController, NovatarBehaviourTree> { }

        [Inject] private readonly NodeGenerator.Factory _nodeGeneratorFactory;

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

            var tree = new BehaviourTreeBuilder()
                .Parallel(nameof(NovatarBehaviourTree), 100, 100)
                    .Selector("RelationshipTreeSelector")
                        .Sequence(EntityState.Spawning.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Spawning))
                            .Sequence("Sequence")
                                .Do(nameof(LightSwitchNode), lightSwitchNode.Tick)
                                .Do(nameof(EnterScreenNode), enterScreenNode.Tick)
                                .Do(nameof(SwitchEntityStateNode), toUnacquaintedStateNode.Tick)
                                .End()
                            .End()
                        .Sequence(EntityState.Unacquainted.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Unacquainted))
                            .Selector("Selector")
                                .Sequence("Sequence")
                                    .Do(nameof(FollowAvatarNode), followAvatarNode.Tick)
                                    .Do(nameof(FirstTouchNode), unacquaintedFirstTouchNode.Tick)
                                    .End()
                                .Sequence("Sequence")
                                    .Do(nameof(IdleTimeoutRandomNode), unacquaintedRandomTimeoutNode.Tick)
                                    .Do(nameof(SwitchEntityStateNode), toNeutralStateNode.Tick)
                                    .End()
                                .End()
                            .End()
                        .Sequence(EntityState.Neutral.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Neutral))
                            .Sequence("Sequence")
                                .Do(nameof(LeaveScreenNode), leaveScreenNode.Tick)
                                .Do(nameof(DeactivateSelfNode), deactivateSelfNode.Tick)
                                .End()
                            .End()
                        .Sequence(EntityState.Friend.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Friend))
                            .Selector("Selector")
                                .Do(nameof(FollowAvatarNode), followAvatarNode.Tick)
                                .Sequence("Sequence")
                                    .Do(nameof(IdleTimeoutNode), friendTimeoutNode.Tick)
                                    .Do(nameof(SwitchEntityStateNode), toNeutralStateNode.Tick)
                                    .End()
                                .End()
                            .End()
                        .Sequence(EntityState.Enemy.ToString())
                            .Condition(nameof(IsEntityState), t => IsEntityState(EntityState.Enemy))
                            .Selector("Selector")
                                .Sequence("Sequence")
                                    .Do(nameof(DamageAvatarNode), damageAvatarNode.Tick)
                                    .Do(nameof(IdleTimeoutNode), enemyTimeoutNode.Tick)
                                    .Do(nameof(SwitchEntityStateNode), toNeutralStateNode.Tick)
                                    .End()
                                .Do(nameof(FollowAvatarNode), followAvatarNode.Tick)
                                .End()
                            .End()
                    .End()
                .End()
                .Build();

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
