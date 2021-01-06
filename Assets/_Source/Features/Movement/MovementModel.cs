using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Movement
{
    public class MovementModel : AbstractDisposable, IMovementModel
    {
        public class Factory : PlaceholderFactory<IActorStateModel, MovementModel> { }

        private readonly MovementDataComponent _movementDataComponent;

        public bool UseDirectMovement => _movementDataComponent.UseDirectMovement;

        public bool HasMoveIntention => _moveIntention.Value.magnitude > _movementDataComponent.MovementDeadZoneMagnitude;

        private readonly ReactiveProperty<Vector2> _moveIntention;
        public IReadOnlyReactiveProperty<Vector2> MoveIntention => _moveIntention;

        public bool HasTurnIntention => _turnIntention.Value.eulerAngles.z > _movementDataComponent.TurnDeadZoneAngle;

        private readonly ReactiveProperty<Quaternion> _turnIntention;
        public IReadOnlyReactiveProperty<Quaternion> TurnIntention => _turnIntention;

        public float MoveSpeed => _movementDataComponent.MovementSpeed;
        public float TurnSpeed => _movementDataComponent.TurnSpeed;
        public float MoveTargetReachedAccuracy => _movementDataComponent.MoveTargetReachedAccuracy;

        public MovementModel(IActorStateModel actorStateModel)
        {
            _movementDataComponent = actorStateModel.Get<MovementDataComponent>();

            _moveIntention = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(Disposer);
            _turnIntention = new ReactiveProperty<Quaternion>(Quaternion.identity).AddTo(Disposer);
        }

        public void ResetIntentions()
        {
            _moveIntention.Value = Vector2.zero;
            _turnIntention.Value = Quaternion.identity;
        }

        public void SetMovementIntention(Vector2 moveTarget)
        {
            var isAboveDeadZone = moveTarget.magnitude > _movementDataComponent.MovementDeadZoneMagnitude;
            moveTarget = isAboveDeadZone ? moveTarget : Vector2.zero;

            _moveIntention.Value = Vector2.ClampMagnitude(moveTarget, 1);
        }

        public void SetTurnIntention(Quaternion rotation)
        {
            var isAboveDeadZone = rotation.eulerAngles.z > _movementDataComponent.TurnDeadZoneAngle;
            rotation = isAboveDeadZone ? rotation : Quaternion.identity;

            _turnIntention.Value = rotation;
        }
    }
}
