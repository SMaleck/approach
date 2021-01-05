using System;

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Decorator node that inverts the success/failure of its child.
    /// </summary>
    public class InverterNode : IParentBehaviourTreeNode // ToDo Add actual decorator interface, that doesn't work with AddChild
    {
        private readonly string name;
        private IBehaviourTreeNode childNode;

        public InverterNode(string name)
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            if (childNode == null)
            {
                throw new ApplicationException("InverterNode must have a child node!");
            }

            var result = childNode.Tick(time);
            if (result == BehaviourTreeStatus.Failure)
            {
                return BehaviourTreeStatus.Success;
            }
            else if (result == BehaviourTreeStatus.Success)
            {
                return BehaviourTreeStatus.Failure;
            }
            else
            {
                return result;
            }
        }

        public void AddChild(IBehaviourTreeNode child)
        {
            if (this.childNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to InverterNode!");
            }

            this.childNode = child;
        }
    }
}