using _Source.Features.Actors.Data;

namespace _Source.Features.ActorEntities.Avatar.Data
{
    public class AvatarData : IHealthData, IMovementData
    {
        private readonly AvatarDataSource _dataSource;

        // ----------------------------- IHealthData
        public int MaxHealth => _dataSource.MaxHealth;

        // ----------------------------- IMovementData
        public bool UseDirectMovement => _dataSource.MovementDataSource.UseDirectMovement;
        public float MoveTargetReachedAccuracy => _dataSource.MovementDataSource.MoveTargetReachedAccuracy;
        public float MovementSpeed => _dataSource.MovementDataSource.MovementSpeed;
        public float MovementDeadZoneMagnitude => _dataSource.MovementDataSource.MovementDeadZoneMagnitude;
        public float TurnSpeed => _dataSource.MovementDataSource.TurnSpeed;
        public float TurnDeadZoneAngle => _dataSource.MovementDataSource.TurnDeadZoneAngle;

        public AvatarData(AvatarDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
