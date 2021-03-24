using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Util;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Avatar
{
    // ToDo V0 Get IMovableEntity to not be implemented on this
    // ToDo V0 No/AvatarFacade should be reduced in scope to a container
    // ToDo V0 Create Movement MonoComponent to handle movement
    // ToDo V0 Position clamping can also be done in  MonoComponent
    public class AvatarFacade : AbstractDisposableFeature, IMovableEntity
    {
        public class Factory : PlaceholderFactory<IMonoEntity, IActorStateModel, AvatarFacade> { }

        private readonly IMonoEntity _entity;
        private readonly IPauseStateModel _pauseStateModel;

        public Transform LocomotionTarget => _entity.LocomotionTarget;
        public Transform RotationTarget => _entity.RotationTarget;

        public string Name => _entity.Name;
        public Vector3 Position => _entity.Position;
        public Quaternion Rotation => _entity.Rotation;

        private readonly SurvivalDataComponent _survivalDataComponent;
        private readonly HealthDataComponent _healthDataComponent;

        public AvatarFacade(
            IMonoEntity entity,
            IActorStateModel actorStateModel,
            IPauseStateModel pauseStateModel)
        {
            _entity = entity;
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

            entity.StartEntity(this.Disposer);

            _healthDataComponent.IsAlive
                .IfFalse()
                .Subscribe(_ => entity.StopEntity())
                .AddTo(Disposer);
        }

        private void OnTimePassed()
        {
            var timePassed = DateTime.Now - _survivalDataComponent.StartedAt.Value;
            _survivalDataComponent.SetSurvivalSeconds(timePassed.TotalSeconds);
        }
    }
}
