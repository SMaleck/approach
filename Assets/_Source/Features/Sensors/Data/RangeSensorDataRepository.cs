using _Source.Data;
using _Source.Entities.Novatar;
using System.Collections.Generic;
using System.Linq;

namespace _Source.Features.Sensors.Data
{
    public class RangeSensorDataRepository : AbstractDataRepository, IRangeSensorData
    {
        private IReadOnlyDictionary<EntityState, RangeSensorDataEntry> _data;

        public RangeSensorDataRepository(RangeSensorDataSource dataSource)
        {
            _data = dataSource.DataEntries
                .ToDictionary(e => e.State);
        }

        public float GetVisualRangeSize(EntityState state)
        {
            return _data[state].VisualRangeColliderSize;
        }
    }
}
