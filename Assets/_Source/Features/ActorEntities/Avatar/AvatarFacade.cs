using System;
using _Source.Features.ActorEntities.Avatar.Config;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Avatar
{
    // ToDo V0 Get IMovableEntity to not be implemented on this
    public class AvatarFacade : AbstractDisposableFeature, IMovableEntity
    {
        public class Factory : PlaceholderFactory<AvatarEntity, IActorStateModel, AvatarFacade> { }

        private readonly AvatarEntity _avatarEntity;
        private readonly AvatarConfig _avatarConfig;
        private readonly ScreenSizeModel _screenSizeModel;
        private readonly IPauseStateModel _pauseStateModel;

        public Transform LocomotionTarget => _avatarEntity.LocomotionTarget;
        public Transform RotationTarget => _avatarEntity.RotationTarget;

        public string Name => _avatarEntity.Name;
        public Vector3 Position => _avatarEntity.Position;
        public Quaternion Rotation => _avatarEntity.Rotation;

        private readonly SurvivalDataComponent _survivalDataComponent;
        private readonly HealthDataComponent _healthDataComponent;

        public AvatarFacade(
            AvatarEntity avatarEntity,
            IActorStateModel actorStateModel,
            AvatarConfig avatarConfig,
            ScreenSizeModel screenSizeModel,
            IPauseStateModel pauseStateModel)
        {
            _avatarEntity = avatarEntity;
            _avatarConfig = avatarConfig;
            _screenSizeModel = screenSizeModel;
            _pauseStateModel = pauseStateModel;

            avatarEntity.Setup(actorStateModel);

            _survivalDataComponent = actorStateModel.Get<SurvivalDataComponent>();
            _healthDataComponent = actorStateModel.Get<HealthDataComponent>();

            _survivalDataComponent.SetStartedAt(DateTime.Now);

            actorStateModel.Get<TransformDataComponent>()
                .SetMonoEntity(_avatarEntity);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Where(_ => !_pauseStateModel.IsPaused.Value)
                .Subscribe(_ => OnTimePassed())
                .AddTo(Disposer);

            Observable.EveryUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value)
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);

            _healthDataComponent.RelativeHealth
                .Subscribe(OnRelativeHealthChanged)
                .AddTo(Disposer);

            avatarEntity.StartEntity(this.Disposer);
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

        private void OnRelativeHealthChanged(double relativeHealth)
        {
            _avatarEntity.HeadLight.intensity = _avatarConfig.MaxLightIntensity * (float)relativeHealth;
        }
    }
}
