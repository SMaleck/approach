using _Source.Entities.Novatar;
using _Source.Features.Actors.Data;
using _Source.Features.Movement.Data;
using UnityEngine;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class MovementDataComponent : AbstractDataComponent, IEntityStateSensitiveDataComponent
    {
        public class Factory : PlaceholderFactory<IMovementData, MovementDataComponent> { }

        private readonly IMovementData _data;

        public float MovementSpeed { get; private set; }
        private float _movementSpeedFactor;

        public float TurnSpeed => _data.TurnSpeed;
        public float TurnDeadZoneAngle => _data.TurnDeadZoneAngle;
        public float MovementDeadZoneMagnitude => _data.MovementDeadZoneMagnitude;

        public bool UseDirectMovement => _data.UseDirectMovement;
        public float MoveTargetReachedAccuracy => _data.MoveTargetReachedAccuracy;

        public bool HasMoveIntention => MoveIntention.magnitude > MovementDeadZoneMagnitude;
        public Vector2 MoveIntention { get; private set; }

        public bool HasTurnIntention => _data.UseDirectMovement || 
                                        TurnIntention.eulerAngles.z > TurnDeadZoneAngle;

        public Quaternion TurnIntention { get; private set; }

        public MovementDataComponent(IMovementData data)
        {
            _data = data;
            _movementSpeedFactor = 1.0f;

            UpdateMovementSpeed();
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

        public void OnRelationshipChanged(EntityState entityState)
        {
            _movementSpeedFactor = _data.GetSpeedFactor(entityState);
            
            UpdateMovementSpeed();
        }

        private void UpdateMovementSpeed()
        {
            MovementSpeed = _data.MovementSpeed * 
                            _movementSpeedFactor;
        }
    }
}
