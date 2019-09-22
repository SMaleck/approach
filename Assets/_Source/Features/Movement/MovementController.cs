using _Source.Entities;
using _Source.Util;
using UnityEngine;
using Zenject;

namespace _Source.Features.Movement
{
    public class MovementController : AbstractDisposable
    {
        public class Factory : PlaceholderFactory<MovementModel, IMonoEntity, MovementController> { }

        private readonly MovementModel _movementModel;
        private readonly IMonoEntity _entity;

        public MovementController(
            MovementModel movementModel,
            IMonoEntity entity)
        {
            _movementModel = movementModel;
            _entity = entity;
        }

        // ToDo CleanUp MovementController
        public void MoveTowards(Vector3 targetPosition)
        {
            FaceTarget(targetPosition);
            _movementModel.SetMovementIntention(targetPosition);
        }

        private void FaceTarget(Vector3 targetPosition)
        {
            var headingToTarget = targetPosition - _entity.Position;

            var rotation = Quaternion.Slerp(
                _entity.Rotation,
                Quaternion.LookRotation(Vector3.forward, headingToTarget),
                _movementModel.TurnSpeed.AsTimeAdjusted());

            _movementModel.SetTurnIntention(rotation);
        }
    }
}
