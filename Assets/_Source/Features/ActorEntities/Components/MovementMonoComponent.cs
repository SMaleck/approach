using _Source.Features.Actors.DataComponents;
using _Source.Util;
using UnityEngine;

namespace _Source.Features.ActorEntities.Components
{
    public class MovementMonoComponent : AbstractMonoComponent, ITickableMonoComponent
    {
        [SerializeField] private MonoEntity _entity;

        // ToDo V2 Once MonoEntity does not need to expose those anymore, can just have them here as serialized
        private Transform _locomotionTarget => _entity.LocomotionTarget;
        private Transform _rotationTarget => _entity.RotationTarget;

        private MovementDataComponent _movementDataComponent;

        public float MoveSpeed => _movementDataComponent.MovementSpeed;
        public float TurnSpeed => _movementDataComponent.TurnSpeed;
        public float MoveTargetReachedAccuracy => _movementDataComponent.MoveTargetReachedAccuracy;

        protected override void OnSetup()
        {
            _movementDataComponent = Actor.Get<MovementDataComponent>();
        }

        public void Tick()
        {
            HandleMoveIntention();
            HandleTurnIntention();

            _movementDataComponent.ResetIntentions();
        }

        private void HandleMoveIntention()
        {
            if (!_movementDataComponent.HasMoveIntention)
            {
                return;
            }

            var timeAdjustedSpeed = MoveSpeed.AsTimeAdjusted();
            var translateTarget = _movementDataComponent.MoveIntention * timeAdjustedSpeed;

            _locomotionTarget.Translate(translateTarget);
        }

        private void HandleTurnIntention()
        {
            if (!_movementDataComponent.HasTurnIntention)
            {
                return;
            }

            var turnRotation = _movementDataComponent.TurnIntention;

            var rotation = Quaternion.Slerp(
                _rotationTarget.rotation,
                turnRotation,
                TurnSpeed.AsTimeAdjusted());

            _rotationTarget.rotation = rotation;
        }
    }
}
