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
        public class Factory : PlaceholderFactory<MonoEntity, IActorStateModel, AvatarFacade> { }

        private readonly MonoEntity _entity;
        private readonly AvatarConfig _avatarConfig;
        private readonly ScreenSizeModel _screenSizeModel;
        private readonly IPauseStateModel _pauseStateModel;

        public Transform LocomotionTarget => _entity.LocomotionTarget;
        public Transform RotationTarget => _entity.RotationTarget;

        public string Name => _entity.Name;
        public Vector3 Position => _entity.Position;
        public Quaternion Rotation => _entity.Rotation;

        private readonly SurvivalDataComponent _survivalDataComponent;
        private readonly HealthDataComponent _healthDataComponent;

        public AvatarFacade(
            MonoEntity entity,
            IActorStateModel actorStateModel,
            AvatarConfig avatarConfig,
            ScreenSizeModel screenSizeModel,
            IPauseStateModel pauseStateModel)
        {
            _entity = entity;
            _avatarConfig = avatarConfig;
            _screenSizeModel = screenSizeModel;
            _pauseStateModel = pauseStateModel;

            entity.Setup(actorStateModel);

            _survivalDataComponent = actorStateModel.Get<SurvivalDataComponent>();
            _healthDataComponent = actorStateModel.Get<HealthDataComponent>();

            _survivalDataComponent.SetStartedAt(DateTime.Now);

            actorStateModel.Get<TransformDataComponent>()
                .SetMonoEntity(_entity);

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

            entity.StartEntity(this.Disposer);

            _healthDataComponent.IsAlive
                .IfFalse()
                .Subscribe(_ => entity.StopEntity())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            KeepWithinScreenBounds();
        }

        private void KeepWithinScreenBounds()
        {
            var clampedX = Mathf.Clamp(Position.x, -_screenSizeModel.WidthExtendUnits, _screenSizeModel.WidthExtendUnits);
            var clampedY = Mathf.Clamp(Position.y, -_screenSizeModel.HeightExtendUnits, _screenSizeModel.HeightExtendUnits);

            _entity.SetPosition(new Vector3(clampedX, clampedY, Position.z));
        }

        private void OnTimePassed()
        {
            var timePassed = DateTime.Now - _survivalDataComponent.StartedAt.Value;
            _survivalDataComponent.SetSurvivalSeconds(timePassed.TotalSeconds);
        }

        private void OnRelativeHealthChanged(double relativeHealth)
        {
            _entity.HeadLight.intensity = _avatarConfig.MaxLightIntensity * (float)relativeHealth;
        }
    }
}
