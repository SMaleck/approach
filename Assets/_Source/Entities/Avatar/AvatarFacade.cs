using _Source.Features.ActorEntities.Avatar.Config;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using _Source.Features.ScreenSize;
using _Source.Util;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Avatar
{
    public class AvatarFacade : AbstractDisposableFeature, IMonoEntity, IDamageReceiver
    {
        public class Factory : PlaceholderFactory<AvatarEntity, AvatarFacade> { }

        private readonly AvatarEntity _avatarEntity;
        private readonly AvatarConfig _avatarConfig;
        private readonly ScreenSizeModel _screenSizeModel;
        private readonly IPauseStateModel _pauseStateModel;

        public Transform LocomotionTarget => _avatarEntity.LocomotionTarget;
        public Transform RotationTarget => _avatarEntity.RotationTarget;
        public bool IsActive => _avatarEntity.IsActive;
        public Vector3 Position => _avatarEntity.Position;
        public Quaternion Rotation => _avatarEntity.Rotation;
        public Vector3 Size => _avatarEntity.Size;
        public string ToDebugString() => _avatarEntity.ToDebugString();

        private readonly SurvivalDataComponent _survivalDataComponent;
        private readonly HealthDataComponent _healthDataComponent;

        public AvatarFacade(
            AvatarEntity avatarEntity,
            AvatarConfig avatarConfig,
            IActorStateModel actorStateModel,
            ScreenSizeModel screenSizeModel,
            IPauseStateModel pauseStateModel)
        {
            _avatarEntity = avatarEntity;
            _avatarConfig = avatarConfig;
            _screenSizeModel = screenSizeModel;
            _pauseStateModel = pauseStateModel;

            _survivalDataComponent = actorStateModel.Get<SurvivalDataComponent>();
            _healthDataComponent = actorStateModel.Get<HealthDataComponent>();

            _survivalDataComponent.SetStartedAt(DateTime.Now);

            actorStateModel.Get<TransformDataComponent>()
                .SetMonoEntity(this);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Where(_ => !_pauseStateModel.IsPaused.Value)
                .Subscribe(_ => OnTimePassed())
                .AddTo(Disposer);

            Observable.EveryUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value)
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);

            _healthDataComponent.Health
                .Subscribe(OnHealthChanged)
                .AddTo(Disposer);
        }

        public void ReceiveDamage(int damageAmount)
        {
            var currentHealth = _healthDataComponent.Health.Value;
            var newHealth = Math.Max(0, currentHealth - damageAmount);

            _healthDataComponent.SetHealth(newHealth);
        }

        private void OnUpdate()
        {
            KeepWithinScreenBounds();
        }

        private void KeepWithinScreenBounds()
        {
            var clampedX = Mathf.Clamp(Position.x, -_screenSizeModel.WidthExtendUnits, _screenSizeModel.WidthExtendUnits);
            var clampedY = Mathf.Clamp(Position.y, -_screenSizeModel.HeightExtendUnits, _screenSizeModel.HeightExtendUnits);

            _avatarEntity.SetPosition(new Vector3(clampedX, clampedY, Position.z));
        }

        private void OnTimePassed()
        {
            var timePassed = DateTime.Now - _survivalDataComponent.StartedAt.Value;
            _survivalDataComponent.SetSurvivalSeconds(timePassed.TotalSeconds);
        }

        private void OnHealthChanged(int health)
        {
            var relativeHealth = health / _avatarConfig.Health;
            _avatarEntity.HeadLight.intensity = _avatarConfig.MaxLightIntensity * (float)relativeHealth;
        }
    }
}
