using _Source.Features.Actors.Data;
using _Source.Features.Sensors;
using _Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    // ToDo V0 all these components need to be disposed with the actor
    public class SensorDataComponent : AbstractDataComponent, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<SensorDataComponent> { }

        private readonly IReadOnlyDictionary<SensorType, SensorStorage> _storages;
        private readonly Dictionary<IActorStateModel, IDisposable> _lifetimeSubscriptions;

        public bool KnowsAvatar => _storages[SensorType.Visual].KnowsAvatar;
        public IActorStateModel Avatar => _storages[SensorType.Visual].Avatar;

        // ToDo V0 Should use the colliders instead
        private readonly float _sqrFollowRange;
        private readonly float _sqrTouchRange;

        public SensorDataComponent(ISensorData sensorData)
        {
            _lifetimeSubscriptions = new Dictionary<IActorStateModel, IDisposable>();

            _sqrFollowRange = Mathf.Pow(sensorData.FollowRange, 2);
            _sqrTouchRange = Mathf.Pow(sensorData.TouchRange, 2);

            _storages = EnumHelper<SensorType>.Iterator
                .ToDictionary(e => e, e => new SensorStorage(e));
        }

        public void Add(IActorStateModel actor, SensorType sensor)
        {
            _storages[sensor].Add(actor);

            if (!_lifetimeSubscriptions.ContainsKey(actor))
            {
                var sub = actor.Get<HealthDataComponent>().IsAlive
                    .IfFalse()
                    .Subscribe(_ => Remove(actor, sensor))
                    .AddTo(Disposer);

                _lifetimeSubscriptions.Add(actor, sub);
            }
        }

        public void Remove(IActorStateModel actor, SensorType sensor)
        {
            _storages[sensor].Remove(actor);

            if (!_lifetimeSubscriptions.ContainsKey(actor))
            {
                _lifetimeSubscriptions[actor].Dispose();
                _lifetimeSubscriptions.Remove(actor);
            }
        }

        public IReadOnlyList<IActorStateModel> GetInRange(SensorType sensor)
        {
            return _storages[sensor].KnownEntities;
        }

        public bool IsInRange(SensorType sensor, IActorStateModel model)
        {
            return _storages[sensor].KnownEntities.Contains(model);
        }

        public bool IsAvatarInRange(SensorType sensor)
        {
            return _storages[sensor].KnowsAvatar;
        }

        public void Reset()
        {
            _storages.Values.ToArray()
                .ForEach(e => e.Reset());

            _lifetimeSubscriptions.Values.ToArray()
                .ForEach(e => e.Dispose());
            _lifetimeSubscriptions.Clear();
        }

        private bool IsInFollowRange(TransformDataComponent self, TransformDataComponent target)
        {
            var sqrDistance = GetSqrDistance(self, target);
            return sqrDistance <= _sqrFollowRange;
        }

        private bool IsInTouchRange(TransformDataComponent self, TransformDataComponent target)
        {
            var sqrDistance = GetSqrDistance(self, target);
            return sqrDistance <= _sqrTouchRange;
        }

        private float GetSqrDistance(TransformDataComponent self, TransformDataComponent target)
        {
            var distance = self.Position - target.Position;
            return distance.sqrMagnitude;
        }
    }
}
