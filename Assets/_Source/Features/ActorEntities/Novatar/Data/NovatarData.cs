using _Source.Features.Actors.Data;

namespace _Source.Features.ActorEntities.Novatar.Data
{
    public class NovatarData : IHealthData 
    {
        private readonly NovatarDataSource _dataSource;

        public int MaxHealth => _dataSource.MaxHealth;

        public NovatarData(NovatarDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
