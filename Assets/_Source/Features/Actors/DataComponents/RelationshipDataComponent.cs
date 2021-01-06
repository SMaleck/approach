﻿using _Source.Entities.Novatar;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class RelationshipDataComponent : AbstractDisposable, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<RelationshipDataComponent> { }

        private readonly ReactiveProperty<EntityState> _relationship;
        public IReadOnlyReactiveProperty<EntityState> Relationship => _relationship;

        public RelationshipDataComponent()
        {
            _relationship = new ReactiveProperty<EntityState>()
                .AddTo(Disposer);

            Reset();
        }

        public void SetRelationship(EntityState value)
        {
            _relationship.Value = value;
        }

        public void Reset()
        {
            SetRelationship(EntityState.Spawning);
        }
    }
}