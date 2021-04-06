using _Source.Entities.Novatar;

namespace _Source.Features.Movement.Data
{
    public interface IMovementData
    {
        bool UseDirectMovement { get; }
        float MoveTargetReachedAccuracy { get; }

        float MovementSpeed { get; }
        float MovementDeadZoneMagnitude { get; }

        float TurnSpeed { get; }
        float TurnDeadZoneAngle { get; }

        float GetSpeedFactor(EntityState state);
    }
}
