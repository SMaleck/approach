﻿using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour.Nodes;
using _Source.Features.NovatarBehaviour.Sensors;
using System.Collections.Generic;
using Zenject;

namespace _Source.Features.NovatarBehaviour
{
    public class NodeGenerator
    {
        public class Factory : PlaceholderFactory<NodeGenerator> { }

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

        private INovatar _novatarEntity;
        private INovatarStateModel _novatarStateModel;
        private ISensorySystem _sensorySystem;
        private MovementController _movementController;

        public List<AbstractNode> GeneratedNodes { get; private set; }


        public void SetupForNovatar(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            ISensorySystem sensorySystem,
            MovementController movementController)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _sensorySystem = sensorySystem;
            _movementController = movementController;

            GeneratedNodes = new List<AbstractNode>();
        }

        public FollowAvatarNode CreateFollowAvatarNode()
        {
            var node = _followAvatarNodeFactory.Create(
                _novatarEntity,
                _sensorySystem,
                _movementController);
            GeneratedNodes.Add(node);

            return node;
        }

        public IdleTimeoutNode CreateIdleTimeoutNode(double timeout)
        {
            var node = _idleTimeoutNodeFactory.Create(timeout);
            GeneratedNodes.Add(node);

            return node;
        }

        public IdleTimeoutRandomNode CreateIdleTimeoutRandomNode(
            double timeout,
            double randomChance)
        {
            var node = _idleTimeoutRandomNodeFactory.Create(timeout, randomChance);
            GeneratedNodes.Add(node);

            return node;
        }

        public FirstTouchNode CreateFirstTouchNode()
        {
            var node = _firstTouchNodeFactory.Create(
                _novatarEntity,
                _novatarStateModel,
                _sensorySystem);
            GeneratedNodes.Add(node);

            return node;
        }

        public SwitchEntityStateNode CreateSwitchEntityStateNode(EntityState targetEntityState)
        {
            var node = _switchEntityStateNodeFactory.Create(
                _novatarEntity,
                targetEntityState);
            GeneratedNodes.Add(node);

            return node;
        }

        public DeactivateSelfNode CreateDeactivateSelfNode()
        {
            var node = _deactivateSelfNodeFactory.Create(
                _novatarEntity);
            GeneratedNodes.Add(node);

            return node;
        }

        public LeaveScreenNode CreateLeaveScreenNode()
        {
            var node = _leaveScreenNodeFactory.Create(
                _novatarEntity,
                _novatarStateModel,
                _movementController);
            GeneratedNodes.Add(node);

            return node;
        }

        public DamageAvatarNode CreateDamageAvatarNode()
        {
            var node = _damageAvatarNodeFactory.Create(
                _sensorySystem);
            GeneratedNodes.Add(node);

            return node;
        }

        public LightSwitchNode CreateLightSwitchNode()
        {
            var node = _lightSwitchNodeFactory.Create(
                _novatarEntity);
            GeneratedNodes.Add(node);

            return node;
        }

        public EnterScreenNode CreateEnterScreenNode()
        {
            var node = _enterScreenNodeFactory.Create(
                _novatarEntity,
                _novatarStateModel,
                _movementController);
            GeneratedNodes.Add(node);

            return node;
        }
    }
}
