namespace _Source.Features.Movement.Data
{
    public interface IMovementData
    {
        float MovementSpeed { get; }
        float MovementDeadZoneMagnitude { get; }

        float TurnSpeed { get; }
        float TurnDeadZoneAngle { get; }
    }
}
