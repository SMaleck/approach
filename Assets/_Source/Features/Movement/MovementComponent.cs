﻿using _Source.Entities;
using _Source.Features.GameRound;
using _Source.Util;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Movement
{
    public class MovementComponent : AbstractDisposable
    {
        public class Factory : PlaceholderFactory<IMonoEntity, IMovementModel, MovementComponent> { }

        private readonly IMonoEntity _monoEntity;
        private readonly IMovementModel _movementModel;
        private readonly IPauseStateModel _pauseStateModel;

        public MovementComponent(
            IMonoEntity monoEntity,
            IMovementModel movementModel,
            IPauseStateModel pauseStateModel)
        {
            _monoEntity = monoEntity;
            _movementModel = movementModel;
            _pauseStateModel = pauseStateModel;

            Observable.EveryUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value)
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            HandleMoveInput();
            HandleTurnInput();
        }

        private void HandleMoveInput()
        {
            if (!_movementModel.HasMoveIntention)
            {
                return;
            }

            var timeAdjustedSpeed = _movementModel.MoveSpeed.AsTimeAdjusted();
            var translateTarget = _movementModel.MoveIntention.Value * timeAdjustedSpeed;

            _monoEntity.LocomotionTarget.Translate(translateTarget);
        }

        private void HandleTurnInput()
        {
            if (!_movementModel.HasTurnIntention)
            {
                return;
            }

            var turnRotation = _movementModel.TurnIntention.Value;

            var rotation = Quaternion.Slerp(
                _monoEntity.RotationTarget.rotation,
                turnRotation,
                _movementModel.TurnSpeed.AsTimeAdjusted());

            _monoEntity.RotationTarget.rotation = rotation;
        }
    }
}
