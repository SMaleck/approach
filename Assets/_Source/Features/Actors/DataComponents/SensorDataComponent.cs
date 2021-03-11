using _Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    // ToDo V2 Should this be it's own concept? i.e. multiple SensorDataComponents
    // ToDo V0 all these components need to be disposed with the actor
    public class SensorDataComponent : AbstractDataComponent, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<SensorDataComponent> { }

        private readonly List<IActorStateModel> _knownEntities;
        public IReadOnlyList<IActorStateModel> KnownEntities => _knownEntities;

        private readonly Dictionary<IActorStateModel, IDisposable> _lifetimeSubscriptions;

        public SensorDataComponent()
        {
            _knownEntities = new List<IActorStateModel>();
            _lifetimeSubscriptions = new Dictionary<IActorStateModel, IDisposable>();
        }

        public void Add(IActorStateModel actor)
        {
            if (!_knownEntities.Contains(actor))
            {
                var sub = actor.Get<HealthDataComponent>().IsAlive
                    .IfFalse()
                    .Subscribe(_ => Remove(actor))
                    .AddTo(Disposer);

                _lifetimeSubscriptions.Add(actor, sub);
                _knownEntities.Add(actor);
            }
        }

        public void Remove(IActorStateModel actor)
        {
            if (_knownEntities.Contains(actor))
            {
                _lifetimeSubscriptions[actor].Dispose();
                _lifetimeSubscriptions.Remove(actor);

                _knownEntities.Remove(actor);
            }
        }

        public void Reset()
        {
            _knownEntities.Clear();

            _lifetimeSubscriptions.Values.ToArray()
                .ForEach(e => e.Dispose());
            _lifetimeSubscriptions.Clear();
        }
    }
}
