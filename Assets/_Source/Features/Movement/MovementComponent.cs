using _Source.Features.GameRound;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Movement
{
    public class MovementComponent : AbstractDisposable
    {
        public class Factory : PlaceholderFactory<IMovableEntity, IMovementModel, MovementComponent> { }

        private readonly IMovableEntity _entity;
        private readonly IMovementModel _movementModel;
        private readonly IPauseStateModel _pauseStateModel;

        public MovementComponent(
            IMovableEntity entity,
            IMovementModel movementModel,
            IPauseStateModel pauseStateModel)
        {
            _entity = entity;
            _movementModel = movementModel;
            _pauseStateModel = pauseStateModel;

            Observable.EveryUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value)
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            HandleMoveIntention();
            HandleTurnIntention();

            _movementModel.ResetIntentions();
        }

        private void HandleMoveIntention()
        {
            if (!_movementModel.HasMoveIntention)
            {
                return;
            }

            var timeAdjustedSpeed = _movementModel.MoveSpeed.AsTimeAdjusted();
            var translateTarget = _movementModel.MoveIntention.Value * timeAdjustedSpeed;

            _entity.LocomotionTarget.Translate(translateTarget);
        }

        private void HandleTurnIntention()
        {
            if (!_movementModel.HasTurnIntention)
            {
                return;
            }

            var turnRotation = _movementModel.TurnIntention.Value;

            var rotation = Quaternion.Slerp(
                _entity.RotationTarget.rotation,
                turnRotation,
                _movementModel.TurnSpeed.AsTimeAdjusted());

            _entity.RotationTarget.rotation = rotation;
        }
    }
}
