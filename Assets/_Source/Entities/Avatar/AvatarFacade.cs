using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using _Source.Util;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Avatar
{
    public class AvatarFacade : AbstractDisposable, IAvatar, IDamageReceiver
    {
        public class Factory : PlaceholderFactory<AvatarEntity, AvatarFacade> { }

        private readonly AvatarEntity _avatarEntity;
        private readonly AvatarConfig _avatarConfig;
        private readonly AvatarStateModel _avatarStateModel;
        private readonly IReadOnlyMovementModel _movementModel;
        private readonly ScreenSizeModel _screenSizeModel;
        private readonly IPauseStateModel _pauseStateModel;

        public bool IsActive => _avatarEntity.IsActive;
        public Vector3 Position => _avatarEntity.Position;
        public Quaternion Rotation => _avatarEntity.Rotation;
        public Vector3 Size => _avatarEntity.Size;

        public AvatarFacade(
            AvatarEntity avatarEntity,
            AvatarConfig avatarConfig,
            AvatarStateModel avatarStateModel,
            IReadOnlyMovementModel movementModel,
            ScreenSizeModel screenSizeModel,
            IPauseStateModel pauseStateModel)
        {
            _avatarEntity = avatarEntity;
            _avatarConfig = avatarConfig;
            _avatarStateModel = avatarStateModel;
            _movementModel = movementModel;
            _screenSizeModel = screenSizeModel;
            _pauseStateModel = pauseStateModel;

            _avatarStateModel.SetStartedAt(DateTime.Now);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Where(_ => !_pauseStateModel.IsPaused.Value)
                .Subscribe(_ => OnTimePassed())
                .AddTo(Disposer);

            Observable.EveryUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value)
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);

            _avatarStateModel.Health
                .Subscribe(OnHealthChanged)
                .AddTo(Disposer);
        }

        public void ReceiveDamage(double damageAmount)
        {
            var currentHealth = _avatarStateModel.Health.Value;
            var newHealth = Math.Max(0, currentHealth - damageAmount);

            _avatarStateModel.SetHealth(newHealth);
        }

        private void OnUpdate()
        {
            HandleMoveInput();
            HandleTurnInput();
            KeepWithinScreenBounds();
        }

        private void HandleMoveInput()
        {
            if (!_movementModel.HasMoveIntention)
            {
                return;
            }

            var timeAdjustedSpeed = _movementModel.MoveSpeed.AsTimeAdjusted();
            var translateTarget = _movementModel.MoveIntention.Value * timeAdjustedSpeed;

            _avatarEntity.transform.Translate(translateTarget);
        }

        private void HandleTurnInput()
        {
            if (!_movementModel.HasTurnIntention)
            {
                return;
            }

            var turnRotation = _movementModel.TurnIntention.Value;

            var rotation = Quaternion.Slerp(
                _avatarEntity.VisualRepresentationTransform.rotation,
                turnRotation,
                _movementModel.TurnSpeed.AsTimeAdjusted());

            _avatarEntity.VisualRepresentationTransform.rotation = rotation;
        }

        private void KeepWithinScreenBounds()
        {
            var clampedX = Mathf.Clamp(Position.x, -_screenSizeModel.WidthExtendUnits, _screenSizeModel.WidthExtendUnits);
            var clampedY = Mathf.Clamp(Position.y, -_screenSizeModel.HeightExtendUnits, _screenSizeModel.HeightExtendUnits);

            _avatarEntity.SetPosition(new Vector3(clampedX, clampedY, Position.z));
        }

        private void OnTimePassed()
        {
            var timePassed = DateTime.Now - _avatarStateModel.StartedAt.Value;
            _avatarStateModel.SetSurvivalSeconds(timePassed.TotalSeconds);
        }

        private void OnHealthChanged(double health)
        {
            var relativeHealth = health / _avatarConfig.Health;
            _avatarEntity.HeadLight.intensity = _avatarConfig.MaxLightIntensity * (float)relativeHealth;
        }
    }
}
