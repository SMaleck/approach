using _Source.Features.Movement.Data;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Movement
{
    public class MovementModel : AbstractDisposable, IMovementModel
    {
        public class Factory : PlaceholderFactory<IMovementData, MovementModel> { }


        private readonly IMovementData _movementData;

        public bool UseDirectMovement => _movementData.UseDirectMovement;

        public bool HasMoveIntention => _moveIntention.Value.magnitude > _movementData.MovementDeadZoneMagnitude;

        private readonly ReactiveProperty<Vector2> _moveIntention;
        public IReadOnlyReactiveProperty<Vector2> MoveIntention => _moveIntention;


        public bool HasTurnIntention => _turnIntention.Value.eulerAngles.z > _movementData.TurnDeadZoneAngle;

        private readonly ReactiveProperty<Quaternion> _turnIntention;
        public IReadOnlyReactiveProperty<Quaternion> TurnIntention => _turnIntention;


        public float MoveSpeed => _movementData.MovementSpeed;
        public float TurnSpeed => _movementData.TurnSpeed;
        public float MoveTargetReachedAccuracy => _movementData.MoveTargetReachedAccuracy;


        public MovementModel(IMovementData movementData)
        {
            _movementData = movementData;
            _moveIntention = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(Disposer);
            _turnIntention = new ReactiveProperty<Quaternion>(Quaternion.identity).AddTo(Disposer);
        }

        public void Reset()
        {
            _moveIntention.Value = Vector2.zero;
            _turnIntention.Value = Quaternion.identity;
        }

        public void SetMovementIntention(Vector2 moveTarget)
        {
            var isAboveDeadZone = moveTarget.magnitude > _movementData.MovementDeadZoneMagnitude;
            moveTarget = isAboveDeadZone ? moveTarget : Vector2.zero;

            _moveIntention.Value = Vector2.ClampMagnitude(moveTarget, 1);
        }

        public void SetTurnIntention(Quaternion rotation)
        {
            var isAboveDeadZone = rotation.eulerAngles.z > _movementData.TurnDeadZoneAngle;
            rotation = isAboveDeadZone ? rotation : Quaternion.identity;

            _turnIntention.Value = rotation;
        }
    }
}
