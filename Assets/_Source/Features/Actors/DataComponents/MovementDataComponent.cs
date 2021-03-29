using _Source.Features.Actors.Data;
using UnityEngine;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    // ToDo V1 Friends Need  greater MoveSpeed
    public class MovementDataComponent : AbstractDataComponent
    {
        public class Factory : PlaceholderFactory<IMovementData, MovementDataComponent> { }

        private readonly IMovementData _data;

        public bool UseDirectMovement => _data.UseDirectMovement;
        public float MoveTargetReachedAccuracy => _data.MoveTargetReachedAccuracy;
        public float MovementSpeed => _data.MovementSpeed;
        public float MovementDeadZoneMagnitude => _data.MovementDeadZoneMagnitude;
        public float TurnSpeed => _data.TurnSpeed;
        public float TurnDeadZoneAngle => _data.TurnDeadZoneAngle;

        public bool HasMoveIntention => MoveIntention.magnitude > MovementDeadZoneMagnitude;
        public Vector2 MoveIntention { get; private set; }

        public bool HasTurnIntention => TurnIntention.eulerAngles.z > TurnDeadZoneAngle;
        public Quaternion TurnIntention { get; private set; }

        public MovementDataComponent(IMovementData data)
        {
            _data = data;
        }

        public void ResetIntentions()
        {
            MoveIntention = Vector2.zero;
            TurnIntention = Quaternion.identity;
        }

        public void SetMovementIntention(Vector2 moveTarget)
        {
            var isAboveDeadZone = moveTarget.magnitude > MovementDeadZoneMagnitude;
            moveTarget = isAboveDeadZone ? moveTarget : Vector2.zero;

            MoveIntention = Vector2.ClampMagnitude(moveTarget, 1);
        }

        public void SetTurnIntention(Quaternion rotation)
        {
            var isAboveDeadZone = rotation.eulerAngles.z > TurnDeadZoneAngle;
            rotation = isAboveDeadZone ? rotation : Quaternion.identity;

            TurnIntention = rotation;
        }
    }
}
