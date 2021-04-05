using _Source.Features.Actors.Data;

namespace _Source.Features.ActorEntities.Avatar.Data
{
    public class AvatarData : IHealthData
    {
        private readonly AvatarDataSource _dataSource;

        // ----------------------------- IHealthData
        public int MaxHealth => _dataSource.MaxHealth;

        public AvatarData(AvatarDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
