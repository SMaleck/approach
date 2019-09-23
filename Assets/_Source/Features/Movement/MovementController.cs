using _Source.Entities;
using _Source.Util;
using UnityEngine;
using Zenject;

namespace _Source.Features.Movement
{
    public class MovementController : AbstractDisposable
    {
        public class Factory : PlaceholderFactory<MovementModel, IMonoEntity, MovementController> { }

        private static readonly Vector3 V3Forward = new Vector3(0, 1, 0);

        private readonly MovementModel _movementModel;
        private readonly IMonoEntity _entity;

        public MovementController(
            MovementModel movementModel,
            IMonoEntity entity)
        {
            _movementModel = movementModel;
            _entity = entity;
        }

        public bool IsMoving()
        {
            return _movementModel.HasMoveIntention || _movementModel.HasTurnIntention;
        }

        public void MoveToTarget(Vector3 targetPosition)
        {
            var turnIntention = GetTurnIntention(targetPosition);
            _movementModel.SetTurnIntention(turnIntention);

            var moveIntention = _movementModel.UseDirectMovement
                ? targetPosition
                : V3Forward;

            _movementModel.SetMovementIntention(moveIntention);
        }

        public bool IsTargetReached(Vector3 target)
        {
            var sqrDistance = (_entity.Position - target).sqrMagnitude;
            return sqrDistance <= Mathf.Pow(_movementModel.MoveTargetReachedAccuracy, 2);
        }

        public void SetEulerAngles(Vector3 targetRotation)
        {
            _entity.RotationTarget.eulerAngles = targetRotation;
        }

        private Quaternion GetTurnIntention(Vector3 targetPosition)
        {
            var headingToTarget = targetPosition - _entity.Position;

            return Quaternion.Slerp(
                _entity.Rotation,
                Quaternion.LookRotation(Vector3.forward, headingToTarget),
                _movementModel.TurnSpeed.AsTimeAdjusted());
        }
    }
}
