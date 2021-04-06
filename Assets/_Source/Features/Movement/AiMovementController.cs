using _Source.Features.Actors;
using UnityEngine;
using Zenject;

namespace _Source.Features.Movement
{
    public class AiMovementController : AbstractMovementController
    {
        public class Factory : PlaceholderFactory<IActorStateModel, AiMovementController> { }

        private static readonly Vector3 V3Forward = new Vector3(0, 1, 0);

        public AiMovementController(IActorStateModel actorStateModel)
            : base(actorStateModel)
        {
        }

        // ToDo V2 public methods below should probably just go to MovementDataComponent
        public void MoveToTarget(Vector3 target)
        {
            var turnIntention = GetTurnIntention(target);
            MovementDataComponent.SetTurnIntention(turnIntention);

            var moveIntention = GetMoveIntention(target);
            MovementDataComponent.SetMovementIntention(moveIntention);
        }

        public bool IsTargetReached(Vector3 target)
        {
            var sqrDistance = (TransformDataComponent.Position - target).sqrMagnitude;
            return sqrDistance <= Mathf.Pow(MovementDataComponent.MoveTargetReachedAccuracy, 2);
        }

        private Quaternion GetTurnIntention(Vector3 target)
        {
            var heading = target - TransformDataComponent.Position;
            return Quaternion.LookRotation(Vector3.forward, heading);
        }

        private Vector3 GetMoveIntention(Vector3 target)
        {
            return MovementDataComponent.UseDirectMovement
                ? target
                : V3Forward;
        }
    }
}
