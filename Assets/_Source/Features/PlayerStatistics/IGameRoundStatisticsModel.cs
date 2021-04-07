using UniRx;

namespace _Source.Features.PlayerStatistics
{
    public interface IGameRoundStatisticsModel
    {
        IReadOnlyReactiveProperty<int> Friends { get; }
        IReadOnlyReactiveProperty<int> Enemies { get; }
        IReadOnlyReactiveProperty<int> Neutral { get; }
        IReadOnlyReactiveProperty<int> FriendsLost { get; }
        void IncrementFriends();
        void IncrementEnemies();
        void IncrementNeutral();
        void IncrementFriendsLost();
    }
}