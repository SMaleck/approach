using BehaviourTreeSystem;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public abstract class AbstractNode : IBehaviourTreeNode
    {
        public abstract BehaviourTreeStatus Tick(TimeData time);
    }
}
