using _Source.Features.Actors.Data;

namespace _Source.Features.ActorEntities.Novatar.Data
{
    public class NovatarData : IHealthData, IDamageData, IMovementData
    {
        private readonly NovatarDataSource _dataSource;

        // ----------------------------- IHealthData
        public int MaxHealth => _dataSource.MaxHealth;

        // ----------------------------- IDamageData
        public int TouchDamage => _dataSource.TouchDamage;

        // ----------------------------- IMovementData
        public bool UseDirectMovement => _dataSource.MovementDataSource.UseDirectMovement;
        public float MoveTargetReachedAccuracy => _dataSource.MovementDataSource.MoveTargetReachedAccuracy;
        public float MovementSpeed => _dataSource.MovementDataSource.MovementSpeed;
        public float MovementDeadZoneMagnitude => _dataSource.MovementDataSource.MovementDeadZoneMagnitude;
        public float TurnSpeed => _dataSource.MovementDataSource.TurnSpeed;
        public float TurnDeadZoneAngle => _dataSource.MovementDataSource.TurnDeadZoneAngle;

        public NovatarData(NovatarDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
