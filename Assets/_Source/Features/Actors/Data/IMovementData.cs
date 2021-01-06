﻿namespace _Source.Features.Actors.Data
{
    public interface IMovementData
    {
        bool UseDirectMovement { get; }
        float MoveTargetReachedAccuracy { get; }

        float MovementSpeed { get; }
        float MovementDeadZoneMagnitude { get; }

        float TurnSpeed { get; }
        float TurnDeadZoneAngle { get; }
    }
}