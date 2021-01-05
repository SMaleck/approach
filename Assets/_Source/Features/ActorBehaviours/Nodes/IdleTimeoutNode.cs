using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class IdleTimeoutNode : AbstractNode, IResettableNode
    {
        public class Factory : PlaceholderFactory<double, IdleTimeoutNode> { }

        private readonly double _timeoutSeconds;
        private float _timePassed;

        public IdleTimeoutNode(double timeoutSeconds)
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
