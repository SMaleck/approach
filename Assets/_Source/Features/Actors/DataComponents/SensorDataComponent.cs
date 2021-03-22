using _Source.Features.Sensors;
using _Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    // ToDo V0 all these components need to be disposed with the actor
    public class SensorDataComponent : AbstractDataComponent, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<SensorDataComponent> { }

        private readonly IReadOnlyDictionary<SensorType, SensorStorage> _storages;
        private readonly Dictionary<IActorStateModel, IDisposable> _lifetimeSubscriptions;

        // Visual is always a greater range than touch, so we can guarantee this to be correct for both cases
        public bool KnowsAvatar => _storages[SensorType.Visual].KnowsAvatar;
        public IActorStateModel Avatar => _storages[SensorType.Visual].Avatar;

        public SensorDataComponent()
        {
            _lifetimeSubscriptions = new Dictionary<IActorStateModel, IDisposable>();

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
    }
}
