using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ActorSensors.Data;
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
        public class Factory : PlaceholderFactory<IActorStateModel, IActorStateModel, IRangeSensorData, SensorySystem> { }

        [Inject] private readonly RangeSensor.Factory _rangeSensorFactory;

        private readonly IActorStateModel _ownerActorStateModel;
        private readonly IActorStateModel _targetActorStateModel;
        private readonly IRangeSensorData _rangeSensorData;
        private readonly IPauseStateModel _pauseStateModel;

        private readonly HealthDataComponent _healthDataComponent;
        private readonly List<ISensor> _sensors;
        private RangeSensor _rangeSensor;

        public SensorySystem(
            IActorStateModel ownerActorStateModel,
            IActorStateModel targetActorStateModel,
            IRangeSensorData rangeSensorData,
            IPauseStateModel pauseStateModel)
        {
            _ownerActorStateModel = ownerActorStateModel;
            _targetActorStateModel = targetActorStateModel;
            _rangeSensorData = rangeSensorData;
            _pauseStateModel = pauseStateModel;

            _healthDataComponent = _ownerActorStateModel.Get<HealthDataComponent>();
            _sensors = new List<ISensor>();
        }

        public void Initialize()
        {
            _rangeSensor = _rangeSensorFactory.Create(
                _ownerActorStateModel,
                _targetActorStateModel,
                _rangeSensorData);

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

        // ToDo V1 Handle Following up to this point and to TouchRange only if needed
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
