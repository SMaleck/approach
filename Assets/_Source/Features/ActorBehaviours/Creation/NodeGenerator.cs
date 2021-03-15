using System.Collections.Generic;
using System.Linq;
using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.Actors;
using _Source.Features.ActorSensors;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Creation
{
    public class NodeGenerator
    {
        public class Factory : PlaceholderFactory<IActorStateModel, ISensorySystem, MovementController, NodeGenerator> { }

        [Inject] private readonly FollowAvatarNode.Factory _followAvatarNodeFactory;
        [Inject] private readonly IdleTimeoutNode.Factory _idleTimeoutNodeFactory;
        [Inject] private readonly IdleTimeoutRandomNode.Factory _idleTimeoutRandomNodeFactory;
        [Inject] private readonly FirstTouchNode.Factory _firstTouchNodeFactory;
        [Inject] private readonly SwitchEntityStateNode.Factory _switchEntityStateNodeFactory;
        [Inject] private readonly DeactivateSelfNode.Factory _deactivateSelfNodeFactory;
        [Inject] private readonly LeaveScreenNode.Factory _leaveScreenNodeFactory;
        [Inject] private readonly DamageAvatarNode.Factory _damageAvatarNodeFactory;
        [Inject] private readonly LightSwitchNode.Factory _lightSwitchNodeFactory;
        [Inject] private readonly EnterScreenNode.Factory _enterScreenNodeFactory;
        [Inject] private readonly MovementNode.Factory _movementNodeFactory;

        private readonly IActorStateModel _actorStateModel;
        private readonly ISensorySystem _sensorySystem;
        private readonly MovementController _movementController;

        private readonly List<AbstractNode> _generatedNodes;

        public NodeGenerator(
            IActorStateModel actorStateModel,
            ISensorySystem sensorySystem,
            MovementController movementController)
        {
            _actorStateModel = actorStateModel;
            _sensorySystem = sensorySystem;
            _movementController = movementController;

            _generatedNodes = new List<AbstractNode>();
        }

        public IBehaviourTreeNode[] GetGeneratedNodes()
        {
            return _generatedNodes
                .Cast<IBehaviourTreeNode>()
                .ToArray();
        }

        public FollowAvatarNode CreateFollowAvatarNode()
        {
            var node = _followAvatarNodeFactory.Create(_actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public IdleTimeoutNode CreateIdleTimeoutNode(double timeout)
        {
            var node = _idleTimeoutNodeFactory.Create(timeout);
            _generatedNodes.Add(node);

            return node;
        }

        public IdleTimeoutRandomNode CreateIdleTimeoutRandomNode(
            double timeout,
            double randomChance)
        {
            var node = _idleTimeoutRandomNodeFactory.Create(timeout, randomChance);
            _generatedNodes.Add(node);

            return node;
        }

        public FirstTouchNode CreateFirstTouchNode()
        {
            var node = _firstTouchNodeFactory.Create(
                _actorStateModel,
                _sensorySystem);
            _generatedNodes.Add(node);

            return node;
        }

        public SwitchEntityStateNode CreateSwitchEntityStateNode(EntityState targetEntityState)
        {
            var node = _switchEntityStateNodeFactory.Create(
                _actorStateModel,
                targetEntityState);
            _generatedNodes.Add(node);

            return node;
        }

        public DeactivateSelfNode CreateDeactivateSelfNode()
        {
            var node = _deactivateSelfNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public LeaveScreenNode CreateLeaveScreenNode()
        {
            var node = _leaveScreenNodeFactory.Create(
                _actorStateModel,
                _movementController);
            _generatedNodes.Add(node);

            return node;
        }

        public DamageAvatarNode CreateDamageAvatarNode()
        {
            var node = _damageAvatarNodeFactory.Create(
                _sensorySystem,
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public LightSwitchNode CreateLightSwitchNode()
        {
            var node = _lightSwitchNodeFactory.Create(
                _actorStateModel);
            _generatedNodes.Add(node);

            return node;
        }

        public EnterScreenNode CreateEnterScreenNode()
        {
            var node = _enterScreenNodeFactory.Create(
                _actorStateModel,
                _movementController);
            _generatedNodes.Add(node);

            return node;
        }

        public MovementNode CreateMovementNode()
        {
            var node = _movementNodeFactory.Create(
                _actorStateModel,
                _movementController);
            _generatedNodes.Add(node);

            return node;
        }
    }
}
