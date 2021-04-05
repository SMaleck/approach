using _Source.Entities.Novatar;
using _Source.Features.Actors;
using _Source.Features.Actors.Data;
using System.Collections.Generic;
using System.Linq;

namespace _Source.Features.Movement.Data
{
    public class MovementDataRepository : IMovementDataRepository
    {
        public class DataAdapter : IMovementData
        {
            private readonly MovementDataSource _dataSource;

            public float MovementSpeed => _dataSource.MovementSpeed;
            public float MovementDeadZoneMagnitude => _dataSource.MovementDeadZoneMagnitude;
            public float TurnSpeed => _dataSource.TurnSpeed;
            public float TurnDeadZoneAngle => _dataSource.TurnDeadZoneAngle;

            public bool UseDirectMovement => _dataSource.UseDirectMovement;
            public float MoveTargetReachedAccuracy => _dataSource.MoveTargetReachedAccuracy;

            private readonly IReadOnlyDictionary<EntityState, MovementDataSource.StateSpeedFactorRow> _speedFactors;

            public DataAdapter(MovementDataSource dataSource)
            {
                _dataSource = dataSource;

                _speedFactors = dataSource.StateSpeedFactors.ToDictionary(
                    e => e.State,
                    e => e);
            }

            public float GetSpeedFactor(EntityState state)
            {
                return _speedFactors[state].SpeedFactor;
            }
        }

        private readonly IReadOnlyDictionary<EntityType, IMovementData> _adapters;

        public IMovementData this[EntityType type] => _adapters[type];

        public MovementDataRepository(List<MovementDataSource> dataSources)
        {
            _adapters = dataSources.ToDictionary(
                e => e.Entity,
                e => (IMovementData)new DataAdapter(e));
        }
    }
}
