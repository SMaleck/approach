using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class IdleTimeoutRandomNode : AbstractNode, IResettableNode
    {
        public class Factory : PlaceholderFactory<double, double, IdleTimeoutRandomNode> { }

        private readonly double _timeoutSeconds;
        private readonly double _randomChance;
        private float _timePassed;

        public IdleTimeoutRandomNode(
            double timeoutSeconds,
            double randomChance)
        {
            _timeoutSeconds = timeoutSeconds;
            _randomChance = randomChance;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            _timePassed += time.DeltaTimeSeconds;

            return _timePassed > _timeoutSeconds
                ? RandomizeAndReset()
                : BehaviourTreeStatus.Running;
        }

        private BehaviourTreeStatus RandomizeAndReset()
        {
            Reset();

            var diceRoll = UnityEngine.Random.Range(0f, 1f);
            return diceRoll <= _randomChance
                ? BehaviourTreeStatus.Success
                : BehaviourTreeStatus.Failure;
        }

        public void Reset()
        {
            _timePassed = 0;
        }
    }
}
