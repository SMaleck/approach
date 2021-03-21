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

        public FollowAvatarNode FollowAvatar()
        {
            var node = _followAvatarNodeFactory.Create(_actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public IdleTimeoutNode IdleTimeout(double timeout)
        {
            var node = _idleTimeoutNodeFactory.Create(timeout);
            _generatedNodes.Add(node);

            return node;
        }

        public IdleTimeoutRandomNode IdleTimeoutRandom(
            double timeout,
            double randomChance)
        {
            var node = _idleTimeoutRandomNodeFactory.Create(timeout, randomChance);
            _generatedNodes.Add(node);

            return node;
        }

        public FirstTouchNode FirstTouch()
        {
            var node = _firstTouchNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public SwitchEntityStateNode SwitchEntityState(EntityState targetEntityState)
        {
            var node = _switchEntityStateNodeFactory.Create(
                _actorStateModel,
                targetEntityState);
            _generatedNodes.Add(node);

            return node;
        }

        public DeactivateSelfNode DeactivateSelf()
        {
            var node = _deactivateSelfNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public LeaveScreenNode LeaveScreen()
        {
            var node = _leaveScreenNodeFactory.Create(
                _actorStateModel,
                _movementController);
            _generatedNodes.Add(node);

            return node;
        }

        public DamageActorNode Damage()
        {
            var node = _damageActorNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public LightSwitchNode LightSwitch()
        {
            var node = _lightSwitchNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public EnterScreenNode EnterScreen()
        {
            var node = _enterScreenNodeFactory.Create(
                _actorStateModel,
                _movementController);
            _generatedNodes.Add(node);

            return node;
        }

        public MovementNode Movement()
        {
            var node = _movementNodeFactory.Create(
                _actorStateModel,
                _movementController);
            _generatedNodes.Add(node);

            return node;
        }

        public FindDamageReceiversNode FindDamageReceiver()
        {
            var node = _findDamageReceiversNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }
    }
}
