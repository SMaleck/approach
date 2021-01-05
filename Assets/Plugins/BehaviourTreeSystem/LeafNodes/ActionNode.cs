using System;

namespace BehaviourTreeSystem.LeafNodes
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode : IBehaviourTreeNode
    {
        private string name;
        private Func<TimeData, BehaviourTreeStatus> fn;

        public ActionNode(string name, Func<TimeData, BehaviourTreeStatus> fn)
        {
            this.name = name;
            this.fn = fn;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            return fn(time);
        }
    }
}