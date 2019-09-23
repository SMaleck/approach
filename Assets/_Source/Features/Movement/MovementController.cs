using _Source.Entities;
using _Source.Util;
using UniRx;
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

        private Vector3 _lastMovementTarget;
        private float _lastTargetAccuracy;

        public MovementController(
            MovementModel movementModel,
            IMonoEntity entity)
        {
            _movementModel = movementModel;
            _entity = entity;

            _lastMovementTarget = _entity.Position;

            Observable.EveryLateUpdate()
                .Where(_ => IsLastTargetReached())
                .Subscribe(_ => Stop())
                .AddTo(Disposer);
        }

        public bool IsMoving()
        {
            return _movementModel.HasMoveIntention || _movementModel.HasTurnIntention;
        }

        // ToDo CleanUp MovementController
        public void MoveToTarget(Vector3 targetPosition)
        {
            MoveToTarget(targetPosition, _movementModel.MoveTargetReachedAccuracy);
        }

        public void MoveToTarget(Vector3 targetPosition, float targetAccuracy)
        {
            _lastMovementTarget = targetPosition;
            _lastTargetAccuracy = targetAccuracy;
            if (IsLastTargetReached())
            {
                Stop();
                return;
            }

            var turnIntention = GetTurnIntention(targetPosition);
            _movementModel.SetTurnIntention(turnIntention);
            
            var moveIntention = _movementModel.UseDirectMovement
                ? targetPosition
                : V3Forward;

            _movementModel.SetMovementIntention(moveIntention);
        }

        public void Stop()
        {
            _movementModel.Reset();
        }

        public bool IsLastTargetReached()
        {
            var sqrDistance = (_entity.Position - _lastMovementTarget).sqrMagnitude;
            return sqrDistance <= Mathf.Pow(_lastTargetAccuracy, 2);
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
