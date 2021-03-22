using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using System.Collections.Generic;

namespace _Source.Features.Sensors
{
    public class SensorStorage
    {
        public SensorType SensoryType { get; }

        private readonly List<IActorStateModel> _knownEntities;
        public IReadOnlyList<IActorStateModel> KnownEntities => _knownEntities;

        public bool KnowsAvatar => Avatar != null;
        public IActorStateModel Avatar { get; private set; }

        public SensorStorage(SensorType sensoryType)
        {
            SensoryType = sensoryType;
            _knownEntities = new List<IActorStateModel>();
        }

        public void Add(IActorStateModel actor)
        {
            if (!_knownEntities.Contains(actor))
            {
                _knownEntities.Add(actor);
                TryAddAsAvatar(actor);
            }
        }

        public void Remove(IActorStateModel actor)
        {
            if (_knownEntities.Contains(actor))
            {
                _knownEntities.Remove(actor);
                TryRemoveAsAvatar(actor);
            }
        }

        public void Reset()
        {
            _knownEntities.Clear();
        }

        private void TryAddAsAvatar(IActorStateModel actor)
        {
            if (actor.Get<EntityTypeDataComponent>().EntityType == EntityType.Avatar)
            {
                Avatar = actor;
            }
        }

        private void TryRemoveAsAvatar(IActorStateModel actor)
        {
            if (actor.Get<EntityTypeDataComponent>().EntityType == EntityType.Avatar)
            {
                Avatar = null;
            }
        }
    }
}
