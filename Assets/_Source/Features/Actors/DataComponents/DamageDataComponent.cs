﻿using _Source.Features.Actors.Data;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class DamageDataComponent : AbstractDataComponent, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<IDamageData, DamageDataComponent> { }

        private readonly IDamageData _data;

        public int Damage => _data.TouchDamage;
        public int HitCount { get; private set; }
        public bool HasDeliveredDamage => HitCount > 0;

        public DamageDataComponent(IDamageData data)
        {
            _data = data;
        }

        public void IncrementHitCount()
        {
            HitCount++;
        }

        public void Reset()
        {
            HitCount = 0;
        }
    }
}
