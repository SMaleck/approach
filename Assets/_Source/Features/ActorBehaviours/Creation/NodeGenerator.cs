using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.Actors;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace _Source.Features.ActorBehaviours.Creation
{
    public class NodeGenerator
    {
        public class Factory : PlaceholderFactory<IActorStateModel, MovementController, NodeGenerator> { }

        [Inject] private readonly FollowAvatarNode.Factory _followAvatarNodeFactory;
        [Inject] private readonly IdleTimeoutNode.Factory _idleTimeoutNodeFactory;
        [Inject] private readonly IdleTimeoutRandomNode.Factory _idleTimeoutRandomNodeFactory;
        [Inject] private readonly FirstTouchNode.Factory _firstTouchNodeFactory;
        [Inject] private readonly SwitchEntityStateNode.Factory _switchEntityStateNodeFactory;
        [Inject] private readonly DeactivateSelfNode.Factory _deactivateSelfNodeFactory;
        [Inject] private readonly LeaveScreenNode.Factory _leaveScreenNodeFactory;
        [Inject] private readonly DamageActorNode.Factory _damageActorNodeFactory;
        [Inject] private readonly LightSwitchNode.Factory _lightSwitchNodeFactory;
        [Inject] private readonly EnterScreenNode.Factory _enterScreenNodeFactory;
        [Inject] private readonly MovementNode.Factory _movementNodeFactory;
        [Inject] private readonly FindDamageReceiversNode.Factory _findDamageReceiversNodeFactory;
        [Inject] private readonly NearDeathNode.Factory _nearDeathNodeFactory;
        [Inject] private readonly WaitNode.Factory _waitNodeFactory;

        private readonly IActorStateModel _actorStateModel;
        private readonly MovementController _movementController;

        private readonly List<AbstractNode> _generatedNodes;

        public NodeGenerator(
            IActorStateModel actorStateModel,
            MovementController movementController)
        {
            _actorStateModel = actorStateModel;
            _movementController = movementController;

            _generatedNodes = new List<AbstractNode>();
        }

        public IBehaviourTreeNode[] GetGeneratedNodes()
        {
            return _generatedNodes
                .Cast<IBehaviourTreeNode>()
                .ToArray();
        }

        public IBehaviourTreeNode FollowAvatar()
        {
            var node = _followAvatarNodeFactory.Create(_actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode IdleTimeout(double timeout)
        {
            var node = _idleTimeoutNodeFactory.Create(timeout);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode IdleTimeoutRandom(
            double timeout,
            double randomChance)
        {
            var node = _idleTimeoutRandomNodeFactory.Create(timeout, randomChance);
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

        public IBehaviourTreeNode SwitchEntityState(EntityState targetEntityState)
        {
            var node = _switchEntityStateNodeFactory.Create(
                _actorStateModel,
                targetEntityState);
            _generatedNodes.Add(node);

            return node;
        }

        public IBehaviourTreeNode DeactivateSelf()
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

        public IBehaviourTreeNode Movement()
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

        public IBehaviourTreeNode Wait(double seconds)
        {
            var node = _waitNodeFactory.Create(seconds);
            _generatedNodes.Add(node);

            return node;
        }
    }
}
