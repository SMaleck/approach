namespace _Source.Features.ActorBehaviours.Data
{
    public interface IStateDelaysData
    {
        double EnemyStaySeconds { get; }
        double FriendPatienceSeconds { get; }
        double NeutralStaySeconds { get; }
    }
}