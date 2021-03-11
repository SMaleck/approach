using BehaviourTreeSystem;

namespace _Source.Features.ActorBehaviours.Creation
{
    public class BehaviourTree
    {
        public IBehaviourTreeNode StartNode { get; }
        public IBehaviourTreeNode[] Nodes { get; }

        public BehaviourTree(
            IBehaviourTreeNode startNode,
            IBehaviourTreeNode[] nodes)
        {
            StartNode = startNode;
            Nodes = nodes;
        }
    }
}
