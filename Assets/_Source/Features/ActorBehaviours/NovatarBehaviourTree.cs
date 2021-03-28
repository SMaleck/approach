using _Source.Features.ActorBehaviours.Creation;
using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.Actors;
using _Source.Features.Movement;
using _Source.Util;
using BehaviourTreeSystem;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorBehaviours
{
    public class NovatarBehaviourTree : AbstractDisposableFeature, IInitializable
    {
        public class Factory : PlaceholderFactory<IActorStateModel, MovementController, NovatarBehaviourTree> { }

        [Inject] private readonly NovatarBehaviourTreeFactory _novatarBehaviourTreeFactory;

        private readonly IActorStateModel _actorStateModel;
        private readonly MovementController _movementController;

        private IBehaviourTreeNode _behaviourTree;
        private List<IResettableNode> _resettableNodes;

        public NovatarBehaviourTree(
            IActorStateModel actorStateModel,
            MovementController movementController)
        {
            _actorStateModel = actorStateModel;
            _movementController = movementController;
        }

        public void Initialize()
        {
            var behaviourTree = _novatarBehaviourTreeFactory.Create(
                _actorStateModel,
                _movementController);

            AggregateNodes(behaviourTree.Nodes);

            _behaviourTree = behaviourTree.StartNode;
        }

        public void Tick()
        {
            _behaviourTree.Tick(new TimeData(Time.deltaTime));
        }

        public void Reset()
        {
            _resettableNodes.ForEach(node => node.Reset());
        }

        private void AggregateNodes(IBehaviourTreeNode[] nodes)
        {
            _resettableNodes = nodes
                .OfType<IResettableNode>()
                .ToList();
        }
    }
}
