using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class IdleTimeoutRandomNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, double, TimeoutDataComponent.Storage, double, IdleTimeoutRandomNode> { }

        private readonly TimeoutDataComponent _timeoutDataComponent;
        private readonly double _timeoutSeconds;
        private readonly TimeoutDataComponent.Storage _storage;
        private readonly double _randomChance;

        public IdleTimeoutRandomNode(
            IActorStateModel actor,
            double timeoutSeconds,
            TimeoutDataComponent.Storage storage,
            double randomChance)
        {
            _timeoutDataComponent = actor.Get<TimeoutDataComponent>();
            _timeoutSeconds = timeoutSeconds;
            _storage = storage;
            _randomChance = randomChance;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            _timeoutDataComponent[_storage] += time.DeltaTimeSeconds;

            return _timeoutDataComponent[_storage] > _timeoutSeconds
                ? RandomizeAndReset()
                : BehaviourTreeStatus.Running;
        }

        private BehaviourTreeStatus RandomizeAndReset()
        {
            _timeoutDataComponent[_storage] = 0;

            var diceRoll = UnityEngine.Random.Range(0f, 1f);
            return diceRoll <= _randomChance
                ? BehaviourTreeStatus.Success
                : BehaviourTreeStatus.Failure;
        }
    }
}
