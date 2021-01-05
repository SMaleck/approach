using System.Collections.Generic;
using _Source.Entities.Novatar;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorBehaviours.Sensors
{
    public class SensorySystem : AbstractDisposable, ISensorySystem, IInitializable
    {
        public class Factory : PlaceholderFactory<INovatar, IActorStateModel, SensorySystem> { }

        [Inject] private readonly RangeSensor.Factory _rangeSensorFactory;

        private readonly INovatar _novatarEntity;
        private readonly NovatarConfig _novatarConfig;
        private readonly IPauseStateModel _pauseStateModel;

        private readonly HealthDataComponent _healthDataComponent;
        private readonly List<ISensor> _sensors;
        private RangeSensor _rangeSensor;

        public SensorySystem(
            INovatar novatarEntity,
            IActorStateModel actorStateModel,
            NovatarConfig novatarConfig,
            IPauseStateModel pauseStateModel)
        {
            _novatarEntity = novatarEntity;
            _novatarConfig = novatarConfig;
            _pauseStateModel = pauseStateModel;

            _healthDataComponent = actorStateModel.Get<HealthDataComponent>();
            _sensors = new List<ISensor>();
        }

        public void Initialize()
        {
            _rangeSensor = _rangeSensorFactory.Create(
                _novatarEntity,
                _novatarConfig.RangeSensorConfig);
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
