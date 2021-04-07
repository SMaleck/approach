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

        private float MoveSpeed => _movementDataComponent.MovementSpeed;
        private float TurnSpeed => _movementDataComponent.TurnSpeed;

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
            
            // ToDo V2 TurnSpeed is applied in a wrong way, as it will be clamped here to [0, 1]
            // Currently looks ok, but higher values may not have the desired effect
            var rotation = Quaternion.Slerp(
                _rotationTarget.rotation,
                turnRotation,
                TurnSpeed.AsTimeAdjusted());

            _rotationTarget.rotation = rotation;
        }
    }
}
