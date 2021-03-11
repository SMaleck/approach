using _Source.Features.ActorBehaviours.Nodes;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ActorSensors;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Util;
using BehaviourTreeSystem;
using System.Collections.Generic;
using System.Linq;
using _Source.Features.ActorBehaviours.Creation;
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

        [Inject] private readonly NovatarBehaviourTreeFactory _novatarBehaviourTreeFactory;

        private readonly IActorStateModel _actorStateModel;
        private readonly ISensorySystem _sensorySystem;
        private readonly MovementController _movementController;
        private readonly IPauseStateModel _pauseStateModel;

        private IBehaviourTreeNode _behaviourTree;
        private List<IResettableNode> _resettableNodes;
        private List<IResettableNode> _resettableTimeoutNodes;

        public NovatarBehaviourTree(
            IActorStateModel actorStateModel,
            ISensorySystem sensorySystem,
            MovementController movementController,
            IPauseStateModel pauseStateModel)
        {
            _actorStateModel = actorStateModel;
            _sensorySystem = sensorySystem;
            _movementController = movementController;
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
            var behaviourTree = _novatarBehaviourTreeFactory.Create(
                _actorStateModel,
                _sensorySystem,
                _movementController);

            AggregateNodes(behaviourTree.Nodes);

            return behaviourTree.StartNode;
        }

        private void AggregateNodes(IBehaviourTreeNode[] nodes)
        {
            _resettableNodes = nodes
                .OfType<IResettableNode>()
                .ToList();

            var timeoutNodes = nodes
                .OfType<IdleTimeoutNode>()
                .Cast<IResettableNode>();

            var timeoutRandomNodes = nodes
                .OfType<IdleTimeoutRandomNode>()
                .Cast<IResettableNode>();

            _resettableTimeoutNodes = new List<IResettableNode>();
            _resettableTimeoutNodes.AddRange(timeoutNodes);
            _resettableTimeoutNodes.AddRange(timeoutRandomNodes);
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
