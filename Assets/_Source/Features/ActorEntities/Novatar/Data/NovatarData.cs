using _Source.Features.Actors.Data;

namespace _Source.Features.ActorEntities.Novatar.Data
{
    public class NovatarData : IHealthData, IDamageData
    {
        private readonly NovatarDataSource _dataSource;

        // ----------------------------- IHealthData
        public int MaxHealth => _dataSource.MaxHealth;

        // ----------------------------- IDamageData
        public int TouchDamage => _dataSource.TouchDamage;

        public NovatarData(NovatarDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
