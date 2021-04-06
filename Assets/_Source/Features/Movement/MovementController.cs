using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using UnityEngine;
using Zenject;

namespace _Source.Features.Movement
{
    public class MovementController : AbstractDisposableFeature
    {
        public class Factory : PlaceholderFactory<IActorStateModel, MovementController> { }

        private static readonly Vector3 V3Forward = new Vector3(0, 1, 0);

        private readonly MovementDataComponent _movementDataComponent;
        private readonly TransformDataComponent _transformDataComponent;

        public MovementController(IActorStateModel actorStateModel)
        {
            _movementDataComponent = actorStateModel.Get<MovementDataComponent>();
            _transformDataComponent = actorStateModel.Get<TransformDataComponent>();
        }

        // ToDo V2 public methods below should probably just go to MovementDataComponent
        public void MoveToTarget(Vector3 targetPosition)
        {
            var turnIntention = GetTurnIntention(targetPosition);
            _movementDataComponent.SetTurnIntention(turnIntention);

            var moveIntention = _movementDataComponent.UseDirectMovement
                ? targetPosition
                : V3Forward;

            _movementDataComponent.SetMovementIntention(moveIntention);
        }

        public bool IsTargetReached(Vector3 target)
        {
            var sqrDistance = (_transformDataComponent.Position - target).sqrMagnitude;
            return sqrDistance <= Mathf.Pow(_movementDataComponent.MoveTargetReachedAccuracy, 2);
        }

        private Quaternion GetTurnIntention(Vector3 targetPosition)
        {
            var headingToTarget = targetPosition - _transformDataComponent.Position;

            return Quaternion.Slerp(
                _transformDataComponent.Rotation,
                Quaternion.LookRotation(Vector3.forward, headingToTarget),
                _movementDataComponent.TurnSpeed.AsTimeAdjusted());
        }
    }
}
