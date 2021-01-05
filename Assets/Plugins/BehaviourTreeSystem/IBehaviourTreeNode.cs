namespace BehaviourTreeSystem
{
    public interface IBehaviourTreeNode
    {
        BehaviourTreeStatus Tick(TimeData time);
    }
}