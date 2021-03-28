﻿using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Data;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using System.Collections.Generic;
using Zenject;

namespace _Source.Features.ActorBehaviours.Creation
{
    // ToDo V1 BT Move around on the playing field, increase leave time
    // ToDo V1 BT Sometimes move towards player
    public class NovatarBehaviourTreeFactory
    {
        [Inject] private readonly BehaviourTreeConfig _behaviourTreeConfig;

        #region Node Factories
        [Inject] private readonly FollowAvatarNode.Factory _followAvatarNodeFactory;
        [Inject] private readonly FollowAvatarBoidNode.Factory _followAvatarBoidNodeFactory;
        [Inject] private readonly IdleTimeoutNode.Factory _idleTimeoutNodeFactory;
        [Inject] private readonly IdleTimeoutRandomNode.Factory _idleTimeoutRandomNodeFactory;
        [Inject] private readonly FirstTouchNode.Factory _firstTouchNodeFactory;
        [Inject] private readonly SwitchEntityStateNode.Factory _switchEntityStateNodeFactory;
        [Inject] private readonly DeactivateSelfNode.Factory _deactivateSelfNodeFactory;
        [Inject] private readonly LeaveScreenNode.Factory _leaveScreenNodeFactory;
        [Inject] private readonly DamageActorNode.Factory _damageActorNodeFactory;
        [Inject] private readonly LightSwitchNode.Factory _lightSwitchNodeFactory;
        [Inject] private readonly EnterScreenNode.Factory _enterScreenNodeFactory;
        [Inject] private readonly MoveNode.Factory _movementNodeFactory;
        [Inject] private readonly FindDamageReceiversNode.Factory _findDamageReceiversNodeFactory;
        [Inject] private readonly NearDeathNode.Factory _nearDeathNodeFactory;
        #endregion

        private IActorStateModel _actorStateModel;
        private MovementController _movementController;
        private List<IBehaviourTreeNode> _generatedNodes;

        public BehaviourTree Create(
            IActorStateModel actor,
            MovementController movementController)
        {
            _actorStateModel = actor;
            _movementController = movementController;
            _generatedNodes = new List<IBehaviourTreeNode>();

            // @formatter:off
            var startNode = new BehaviourTreeBuilder()
                .Selector()
                    .Splice(SpawningTree(actor))
                    .Splice(UnacquaintedTree(actor))
                    .Splice(NeutralTree(actor))
                    .Splice(FriendTree(actor))
                    .Splice(EnemyTree(actor))
                    .End()
                .Build();
            // @formatter:on

            return new BehaviourTree(startNode, _generatedNodes.ToArray());
        }

        #region SubTrees

        private IBehaviourTreeNode SpawningTree(IActorStateModel actor)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(actor, EntityState.Spawning))
                    .Sequence()
                        .Do(LightSwitch())
                        .Do(EnterScreen())
                        .Do(SwitchStateTo(EntityState.Unacquainted))
                        .End()
                    .End()
                .Build();
            // @formatter:on
        }

        private IBehaviourTreeNode UnacquaintedTree(IActorStateModel actor)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(actor, EntityState.Unacquainted))
                    .Parallel(1, 1)
                        .Sequence()
                            .Do(UnacquaintedTimeout())
                            .Do(SwitchStateTo(EntityState.Neutral))
                            .End()
                        .Selector()
                            .Sequence()
                                .Do(FollowAvatar())
                                .Do(Move())
                                .Do(FirstTouch())
                                .End()
                            .End() // END Top Selector
                        .End() // END Parallel
                    .End() // END Top Sequence
                .Build();
            // @formatter:on
        }

        private IBehaviourTreeNode NeutralTree(IActorStateModel actor)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(actor, EntityState.Neutral))
                    .Sequence()
                        .Do(LeaveScreen())
                        .Do(Deactivate())
                        .End()
                    .End()
                .Build();
            // @formatter:on
        }

        private IBehaviourTreeNode FriendTree(IActorStateModel actor)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(actor, EntityState.Friend))
                    .Selector()
                        .Sequence()
                            .Do(NearDeath())
                            .Do(SwitchStateTo(EntityState.Neutral))
                            .End()
                        .Sequence()
                            .Do(FollowAvatarBoid())
                            .Do(Move())
                            .End()
                        .Sequence()
                            .Do(FriendTimeout())
                            .Do(SwitchStateTo(EntityState.Neutral))
                            .End()
                        .End()
                    .End()
                .Build();
            // @formatter:on
        }

        private IBehaviourTreeNode EnemyTree(IActorStateModel actor)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(actor, EntityState.Enemy))
                    .Selector()
                        .Sequence()
                            .Do(FindDamageReceiver())
                            .Do(Damage())
                            .End()
                        .Sequence()
                            .Do(EnemyTimeout())
                            .Do(SwitchStateTo(EntityState.Neutral))
                            .End()
                        .End()
                    .End()
                .Build();
            // @formatter:on
        }

        #endregion

        #region Convenience Methods

        private bool IsEntityState(IActorStateModel model, EntityState status)
        {
            var relationshipDataComponent = model.Get<RelationshipDataComponent>();
            return relationshipDataComponent.Relationship.Value == status;
        }

        private IBehaviourTreeNode UnacquaintedTimeout()
        {
            return IdleTimeoutRandom(
                _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds,
                TimeoutDataComponent.Storage.IdleUnacquainted,
                _behaviourTreeConfig.UnacquaintedConfig.TimeBasedSwitchChance);
        }

        private IBehaviourTreeNode EnemyTimeout()
        {
            return IdleTimeout(
                _behaviourTreeConfig.EnemyLeavingTimeoutSeconds,
                TimeoutDataComponent.Storage.IdleEnemy);
        }

        private IBehaviourTreeNode FriendTimeout()
        {
            return IdleTimeout(
                _behaviourTreeConfig.MaxSecondsToFallBehind,
                TimeoutDataComponent.Storage.IdleFriend);
        }

        #endregion

        #region Node Creation

        public IBehaviourTreeNode FollowAvatar()
        {
            var node = _followAvatarNodeFactory.Create(_actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }
        
        public IBehaviourTreeNode FollowAvatarBoid()
        {
            var node = _followAvatarBoidNodeFactory.Create(_actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode IdleTimeout(double timeout, TimeoutDataComponent.Storage storage)
        {
            var node = _idleTimeoutNodeFactory.Create(
                _actorStateModel,
                timeout,
                storage);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode IdleTimeoutRandom(
            double timeout,
            TimeoutDataComponent.Storage storage,
            double randomChance)
        {
            var node = _idleTimeoutRandomNodeFactory.Create(
                _actorStateModel,
                timeout,
                storage,
                randomChance);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode FirstTouch()
        {
            var node = _firstTouchNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode SwitchStateTo(EntityState targetEntityState)
        {
            var node = _switchEntityStateNodeFactory.Create(
                _actorStateModel,
                targetEntityState);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode Deactivate()
        {
            var node = _deactivateSelfNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode LeaveScreen()
        {
            var node = _leaveScreenNodeFactory.Create(
                _actorStateModel,
                _movementController);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode Damage()
        {
            var node = _damageActorNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode LightSwitch()
        {
            var node = _lightSwitchNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode EnterScreen()
        {
            var node = _enterScreenNodeFactory.Create(
                _actorStateModel,
                _movementController);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode Move()
        {
            var node = _movementNodeFactory.Create(
                _actorStateModel,
                _movementController);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode FindDamageReceiver()
        {
            var node = _findDamageReceiversNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode NearDeath()
        {
            var node = _nearDeathNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        #endregion
    }
}
