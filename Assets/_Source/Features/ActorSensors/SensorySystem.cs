using _Source.Entities;
using _Source.Entities.Avatar;
using _Source.Features.ActorEntities.Novatar.Data;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using _Source.Util;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorSensors
{
    public class SensorySystem : AbstractDisposable, ISensorySystem, IInitializable
    {
        public class Factory : PlaceholderFactory<IMonoEntity, IActorStateModel, SensorySystem> { }

        [Inject] private readonly RangeSensor.Factory _rangeSensorFactory;

        private readonly IMonoEntity _ownerEntity;
        private readonly IAvatar _avatarEntity;
        private readonly NovatarData _novatarData;
        private readonly IPauseStateModel _pauseStateModel;

        private readonly HealthDataComponent _healthDataComponent;
        private readonly List<ISensor> _sensors;
        private RangeSensor _rangeSensor;

        public SensorySystem(
            IMonoEntity ownerEntity,
            IActorStateModel actorStateModel,
            IAvatar avatarEntity, // ToDo V0 Don't get implicitly
            NovatarData novatarData,
            IPauseStateModel pauseStateModel)
        {
            _ownerEntity = ownerEntity;
            _avatarEntity = avatarEntity;
            _novatarData = novatarData;
            _pauseStateModel = pauseStateModel;

            _healthDataComponent = actorStateModel.Get<HealthDataComponent>();
            _sensors = new List<ISensor>();
        }

        public void Initialize()
        {
            _rangeSensor = _rangeSensorFactory.Create(
                _novatarData,
                _ownerEntity,
                _avatarEntity);

            _sensors.Add(_rangeSensor);

            Observable.EveryLateUpdate()
                .Where(_ => !_pauseStateModel.IsPaused.Value && _healthDataComponent.IsAlive.Value)
                .Subscribe(_ => _sensors.ForEach(sensor => sensor.UpdateSensorReadings()))
                .AddTo(Disposer);
        }

        public Vector3 GetAvatarPosition()
        {
            return _rangeSensor.GetAvatarPosition();
        }

        public bool IsInFollowRange()
        {
            return _rangeSensor.IsInFollowRange();
        }

        // ToDo Handle Following up to this point and to TouchRange only if needed
        public bool IsInInteractionRange()
        {
            return _rangeSensor.IsInInteractionRange();
        }

        public bool IsInTouchRange()
        {
            return _rangeSensor.IsInTouchRange();
        }
    }
}
