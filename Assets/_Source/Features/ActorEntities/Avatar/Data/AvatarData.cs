using _Source.Entities.Actors.Data;

namespace _Source.Entities.ActorEntities.Avatar.Data
{
    public class AvatarData : IHealthData 
    {
        private readonly AvatarDataSource _dataSource;

        public int MaxHealth => _dataSource.MaxHealth;

        public AvatarData(AvatarDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
