using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class IdleTimeoutNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, double, TimeoutDataComponent.Storage, IdleTimeoutNode> { }

        private readonly TimeoutDataComponent _timeoutDataComponent;
        private readonly double _timeoutSeconds;
        private readonly TimeoutDataComponent.Storage _storage;

        public IdleTimeoutNode(
            IActorStateModel actor,
            double timeoutSeconds,
            TimeoutDataComponent.Storage storage)
        {
            _timeoutDataComponent = actor.Get<TimeoutDataComponent>();
            _timeoutSeconds = timeoutSeconds;
            _storage = storage;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            _timeoutDataComponent[_storage] += time.DeltaTimeSeconds;

            return _timeoutDataComponent[_storage] > _timeoutSeconds
                ? BehaviourTreeStatus.Success
                : BehaviourTreeStatus.Running;
        }
    }
}
