using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class WaitNode : AbstractNode, IResettableNode
    {
        public class Factory : PlaceholderFactory<double, WaitNode> { }

        private readonly double _timeoutSeconds;
        private float _timePassed;

        public WaitNode(double timeoutSeconds)
        {
            _timeoutSeconds = timeoutSeconds;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            _timePassed += time.DeltaTimeSeconds;

            return _timePassed > _timeoutSeconds
                ? BehaviourTreeStatus.Success
                : BehaviourTreeStatus.Running;
        }

        public void Reset()
        {
            _timePassed = 0;
        }
    }
}
