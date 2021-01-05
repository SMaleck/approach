using System.Collections.Generic;

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode : IParentBehaviourTreeNode
    {
        private readonly string name;
        private readonly List<IBehaviourTreeNode> children;

        public SequenceNode(string name)
        {
            this.name = name;
            children = new List<IBehaviourTreeNode>();
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Success)
                {
                    return childStatus;
                }
            }

            return BehaviourTreeStatus.Success;
        }

        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}