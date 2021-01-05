using System.Collections.Generic;

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : IParentBehaviourTreeNode
    {
        private readonly string name;
        private readonly List<IBehaviourTreeNode> children;

        public SelectorNode(string name)
        {
            this.name = name;
            children = new List<IBehaviourTreeNode>();
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Failure)
                {
                    return childStatus;
                }
            }

            return BehaviourTreeStatus.Failure;
        }

        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}