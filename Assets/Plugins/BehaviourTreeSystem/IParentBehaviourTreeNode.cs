namespace BehaviourTreeSystem
{
    /// <summary>
    /// Structural Node the manages execution of downstream nodes
    /// </summary>
    public interface IParentBehaviourTreeNode : IBehaviourTreeNode
    {
        void AddChild(IBehaviourTreeNode child);
    }
}