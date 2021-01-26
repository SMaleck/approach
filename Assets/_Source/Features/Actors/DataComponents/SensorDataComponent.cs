using _Source.Util;
using System.Collections.Generic;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    // ToDo V2 Should this be it's own concept? i.e. multiple SensorDataComponents
    public class SensorDataComponent : AbstractDisposable, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<SensorDataComponent> { }

        private readonly List<IActorStateModel> _knownEntities;
        public IReadOnlyList<IActorStateModel> KnownEntities => _knownEntities;

        public SensorDataComponent()
        {
            _knownEntities = new List<IActorStateModel>();
        }

        public void Add(IActorStateModel actor)
        {
            if (!_knownEntities.Contains(actor))
            {
                _knownEntities.Add(actor);
            }
        }

        public void Remove(IActorStateModel actor)
        {
            if (_knownEntities.Contains(actor))
            {
                _knownEntities.Remove(actor);
            }
        }

        public void Reset()
        {
            _knownEntities.Clear();
        }
    }
}
