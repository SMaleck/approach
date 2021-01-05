namespace BehaviourTreeSystem.StructuralNodes
{
    /// <summary>
    /// Structural Node the manages execution of downstream nodes
    /// </summary>
    public interface IStructuralBehaviourTreeNode : IBehaviourTreeNode
    {
        void AddChild(IBehaviourTreeNode child);
    }
}