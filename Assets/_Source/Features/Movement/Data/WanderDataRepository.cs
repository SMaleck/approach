using _Source.Data;

namespace _Source.Features.Movement.Data
{
    public class WanderDataRepository : AbstractDataRepository, IWanderData
    {
        private readonly WanderDataEntry _data;

        public double IdleSeconds => _data.IdleSeconds;
        public float MinDistance => _data.MinDistance;
        public float MaxDistance => _data.MaxDistance;

        public WanderDataRepository(WanderDataSource dataSource)
        {
            _data = dataSource.DataEntries[0];
        }
    }
}
