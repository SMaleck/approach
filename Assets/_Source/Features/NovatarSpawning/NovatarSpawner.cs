using _Source.Entities.Novatar;
using _Source.Features.GameWorld;
using _Source.Features.NovatarBehaviour;
using _Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using _Source.Features.NovatarSpawning.Data;
using UniRx;
using UnityEngine;

namespace _Source.Features.NovatarSpawning
{
    public class NovatarSpawner : AbstractDisposable
    {
        private readonly NovatarSpawnerConfig _novatarSpawnerConfig;
        private readonly NovatarEntity.Factory _novatarEntityFactory;
        private readonly NovatarStateModel.Factory _novatarStateModelFactory;
        private readonly NovatarConfig _novatarConfig;
        private readonly NovatarPoolItem.Factory _novatarPoolItemFactory;
        private readonly NovatarBehaviourTree.Factory _novatarBehaviourTreeFactory;
        private readonly ScreenSizeController _screenSizeController;

        private readonly List<IEntityPoolItem> _novatarPool;

        public NovatarSpawner(
            NovatarSpawnerConfig novatarSpawnerConfig,
            NovatarConfig novatarConfig,
            NovatarPoolItem.Factory novatarPoolItemFactory,
            NovatarEntity.Factory novatarEntityFactory,
            NovatarStateModel.Factory novatarStateModelFactory,
            NovatarBehaviourTree.Factory novatarBehaviourTreeFactory,
            ScreenSizeController screenSizeController)
        {
            _novatarSpawnerConfig = novatarSpawnerConfig;
            _novatarConfig = novatarConfig;
            _novatarPoolItemFactory = novatarPoolItemFactory;
            _novatarEntityFactory = novatarEntityFactory;
            _novatarStateModelFactory = novatarStateModelFactory;
            _novatarBehaviourTreeFactory = novatarBehaviourTreeFactory;
            _screenSizeController = screenSizeController;

            _novatarPool = new List<IEntityPoolItem>();

            Observable.Interval(TimeSpan.FromSeconds(_novatarSpawnerConfig.SpawnIntervalSeconds))
                .Subscribe(_ => Spawn())
                .AddTo(Disposer);
        }

        private void Spawn()
        {
            var activeItemCount = _novatarPool.Count(item => !item.IsFree);
            if (activeItemCount >= _novatarSpawnerConfig.MaxActiveSpawns)
            {
                return;
            }

            var novatarPoolItem = GetFreeEntity();

            var spawnPosition = GetSpawnPosition(novatarPoolItem.NovatarEntity);
            novatarPoolItem.Reset(spawnPosition);
        }

        private IEntityPoolItem GetFreeEntity()
        {
            var freeItem = _novatarPool.FirstOrDefault(item => item.IsFree);
            if (freeItem != null)
            {
                return freeItem;
            }

            return CreateEntity();
        }

        private IEntityPoolItem CreateEntity()
        {
            var novatarEntity = _novatarEntityFactory.Create(
                _novatarConfig.NovatarPrefab);

            var novatarStateModel = _novatarStateModelFactory
                .Create()
                .AddTo(Disposer);

            novatarEntity.Setup(
                novatarStateModel);

            _novatarBehaviourTreeFactory
                .Create(novatarEntity, novatarStateModel)
                .AddTo(Disposer)
                .Initialize();

            var novatarPoolItem = _novatarPoolItemFactory.Create(
                novatarEntity,
                novatarStateModel);

            _novatarPool.Add(novatarPoolItem);

            return novatarPoolItem;
        }

        private Vector3 GetSpawnPosition(NovatarEntity novatar)
        {
            var spawnSideInt = UnityEngine.Random.Range(0, 4);
            var spawnSide = (ScreenSide)spawnSideInt;

            return _screenSizeController.GetRandomizedOutOfBoundsPosition(
                spawnSide,
                novatar.Size);
        }
    }
}
