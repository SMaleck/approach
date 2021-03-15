﻿using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class OriginDataComponent : AbstractDataComponent
    {
        public class Factory : PlaceholderFactory<OriginDataComponent> { }

        private readonly ReactiveProperty<Vector3> _spawnPosition;
        public IReadOnlyReactiveProperty<Vector3> SpawnPosition => _spawnPosition;

        public OriginDataComponent()
        {
            _spawnPosition = new ReactiveProperty<Vector3>().AddTo(Disposer);
        }

        public void SetSpawnPosition(Vector3 value)
        {
            _spawnPosition.Value = value;
        }
    }
}
