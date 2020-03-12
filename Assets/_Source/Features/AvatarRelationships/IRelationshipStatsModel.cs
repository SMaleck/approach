using UniRx;

namespace _Source.Features.AvatarRelationships
{
    public interface IRelationshipStatsModel
    {
        IReadOnlyReactiveProperty<int> EncountersCount { get; }
        IReadOnlyReactiveProperty<int> FriendsGainedCount { get; }
        IReadOnlyReactiveProperty<int> FriendsLostCount { get; }
        IReadOnlyReactiveProperty<int> EnemiesGainedCount { get; }
    }
}