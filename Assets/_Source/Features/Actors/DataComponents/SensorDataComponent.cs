using _Source.Features.ActorSensors.Data;
using _Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
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

        public bool KnowsAvatar => Avatar != null;
        public IActorStateModel Avatar { get; private set; }

        private readonly Dictionary<IActorStateModel, IDisposable> _lifetimeSubscriptions;

        // ToDo V0 Should use the colliders instead
        private readonly float _sqrFollowRange;
        private readonly float _sqrTouchRange;

        public SensorDataComponent(IRangeSensorData rangeData)
        {
            _knownEntities = new List<IActorStateModel>();
            _lifetimeSubscriptions = new Dictionary<IActorStateModel, IDisposable>();
            _sqrFollowRange = Mathf.Pow(rangeData.FollowRange, 2);
            _sqrTouchRange = Mathf.Pow(rangeData.TouchRange, 2);
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
                TryAddAsAvatar(actor);
            }
        }

        public void Remove(IActorStateModel actor)
        {
            if (_knownEntities.Contains(actor))
            {
                _lifetimeSubscriptions[actor].Dispose();
                _lifetimeSubscriptions.Remove(actor);

                _knownEntities.Remove(actor);
                TryRemoveAsAvatar(actor);
            }
        }

        public bool IsInFollowRange(TransformDataComponent self, TransformDataComponent target)
        {
            var sqrDistance = GetSqrDistance(self, target);
            return sqrDistance <= _sqrFollowRange;
        }

        public bool IsInTouchRange(TransformDataComponent self, TransformDataComponent target)
        {
            var sqrDistance = GetSqrDistance(self, target);
            return sqrDistance <= _sqrTouchRange;
        }

        private void TryAddAsAvatar(IActorStateModel actor)
        {
            if (actor.Get<EntityTypeDataComponent>().EntityType == EntityType.Avatar)
            {
                Avatar = actor;
            }
        }

        private float GetSqrDistance(TransformDataComponent self, TransformDataComponent target)
        {
            var distance = self.Position - target.Position;
            return distance.sqrMagnitude;
        }

        private void TryRemoveAsAvatar(IActorStateModel actor)
        {
            if (actor.Get<EntityTypeDataComponent>().EntityType == EntityType.Avatar)
            {
                Avatar = null;
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
