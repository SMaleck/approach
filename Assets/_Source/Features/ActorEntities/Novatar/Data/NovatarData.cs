using _Source.Features.Actors.Data;

namespace _Source.Features.ActorEntities.Novatar.Data
{
    public class NovatarData : IHealthData, IDamageData, IWanderData
    {
        private readonly NovatarDataSource _dataSource;

        // ----------------------------- IHealthData
        public int MaxHealth => _dataSource.MaxHealth;

        // ----------------------------- IDamageData
        public int TouchDamage => _dataSource.TouchDamage;

        // ----------------------------- IWanderData
        public float WanderMinDistance => _dataSource.WanderDataSource.WanderMinDistance;
        public float WanderMaxDistance => _dataSource.WanderDataSource.WanderMaxDistance;

        public NovatarData(NovatarDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
