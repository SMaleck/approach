using _Source.Util;
using FluentBehaviourTree;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public abstract class AbstractNode : IBehaviourTreeNode
    {
        public abstract BehaviourTreeStatus Tick(TimeData time);
    }
}
