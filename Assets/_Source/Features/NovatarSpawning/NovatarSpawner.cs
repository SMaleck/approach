using _Source.Entities;
using _Source.Entities.Novatar;
using _Source.Features.GameWorld;
using _Source.Features.NovatarBehaviour;
using _Source.Features.NovatarSpawning.Data;
using _Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace _Source.Features.NovatarSpawning
{
    public class NovatarSpawner : AbstractDisposable
    {
        private readonly NovatarSpawnerConfig _novatarSpawnerConfig;
        private readonly NovatarConfig _novatarConfig;
        private readonly NovatarFacade.Factory _novatarFacadeFactory;
        private readonly NovatarEntity.Factory _novatarEntityFactory;
        private readonly NovatarStateModel.Factory _novatarStateModelFactory;
        private readonly NovatarBehaviourTree.Factory _novatarBehaviourTreeFactory;
        private readonly ScreenSizeController _screenSizeController;

        private readonly List<IEntityPoolItem<NovatarEntity>> _novatarPool;

        public NovatarSpawner(
            NovatarSpawnerConfig novatarSpawnerConfig,
            NovatarConfig novatarConfig,
            NovatarFacade.Factory novatarFacadeFactory,
            NovatarEntity.Factory novatarEntityFactory,
            NovatarStateModel.Factory novatarStateModelFactory,
            NovatarBehaviourTree.Factory novatarBehaviourTreeFactory,
            ScreenSizeController screenSizeController)
        {
            _novatarSpawnerConfig = novatarSpawnerConfig;
            _novatarConfig = novatarConfig;
            _novatarFacadeFactory = novatarFacadeFactory;
            _novatarEntityFactory = novatarEntityFactory;
            _novatarStateModelFactory = novatarStateModelFactory;
            _novatarBehaviourTreeFactory = novatarBehaviourTreeFactory;
            _screenSizeController = screenSizeController;

            _novatarPool = new List<IEntityPoolItem<NovatarEntity>>();

            Observable.Interval(TimeSpan.FromSeconds(_novatarSpawnerConfig.SpawnIntervalSeconds))
                .Subscribe(_ => Spawn())
                .AddTo(Disposer);
        }

        public void Spawn()
        {
            var activeItemCount = _novatarPool.Count(item => !item.IsFree);
            if (activeItemCount >= _novatarSpawnerConfig.MaxActiveSpawns)
            {
                return;
            }

            var novatarPoolItem = GetFreeEntity();

            var spawnPosition = GetSpawnPosition(novatarPoolItem.Entity);
            novatarPoolItem.Reset(spawnPosition);
        }

        private IEntityPoolItem<NovatarEntity> GetFreeEntity()
        {
            var freeItem = _novatarPool.FirstOrDefault(item => item.IsFree);
            if (freeItem != null)
            {
                return freeItem;
            }

            return CreateEntity();
        }

        private IEntityPoolItem<NovatarEntity> CreateEntity()
        {
            var novatarEntity = _novatarEntityFactory.Create(
                _novatarConfig.NovatarPrefab);

            novatarEntity.gameObject.name = $"{nameof(novatarEntity)} [{_novatarPool.Count - 1}]";

            var novatarStateModel = _novatarStateModelFactory
                .Create()
                .AddTo(Disposer); // ToDo needed?

            var novatarFacade = _novatarFacadeFactory.Create(
                novatarEntity,
                novatarStateModel);

            _novatarBehaviourTreeFactory
                .Create(novatarFacade, novatarStateModel)
                .AddTo(Disposer)
                .Initialize();

            _novatarPool.Add(novatarFacade);

            return novatarFacade;
        }

        private Vector3 GetSpawnPosition(AbstractMonoEntity entity)
        {
            var spawnSideInt = UnityEngine.Random.Range(0, 4);
            var spawnSide = (ScreenSide)spawnSideInt;

            return _screenSizeController.GetRandomizedOutOfBoundsPosition(
                spawnSide,
                entity.Size,
                _novatarSpawnerConfig.SpawnPositionOffset);
        }
    }
}
