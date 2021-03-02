using _Source.Features.Actors.Data;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class MovementDataComponent : AbstractDataComponent, IDataComponent
    {
        public class Factory : PlaceholderFactory<IMovementData, MovementDataComponent> { }

        private readonly IMovementData _data;

        public bool UseDirectMovement => _data.UseDirectMovement;
        public float MoveTargetReachedAccuracy => _data.MoveTargetReachedAccuracy;
        public float MovementSpeed => _data.MovementSpeed;
        public float MovementDeadZoneMagnitude => _data.MovementDeadZoneMagnitude;
        public float TurnSpeed => _data.TurnSpeed;
        public float TurnDeadZoneAngle => _data.TurnDeadZoneAngle;

        public MovementDataComponent(IMovementData data)
        {
            _data = data;
        }
    }
}
