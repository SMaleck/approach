using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class ResetTimeoutNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, TimeoutDataComponent.Storage, ResetTimeoutNode> { }

        private readonly TimeoutDataComponent.Storage _storage;
        private readonly TimeoutDataComponent _timeoutDataComponent;

        public ResetTimeoutNode(
            IActorStateModel actorStateModel,
            TimeoutDataComponent.Storage storage)
        {
            _storage = storage;
            _timeoutDataComponent = actorStateModel.Get<TimeoutDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            _timeoutDataComponent.ResetTimeout(_storage);
            return BehaviourTreeStatus.Success;
        }
    }
}
