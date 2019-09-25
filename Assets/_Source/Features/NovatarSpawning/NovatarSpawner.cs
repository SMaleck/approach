using _Source.Entities;
using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour;
using _Source.Features.NovatarBehaviour.Sensors;
using _Source.Features.NovatarSpawning.Data;
using _Source.Features.ScreenSize;
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
        private readonly MovementModel.Factory _movementModelFactory;
        private readonly MovementController.Factory _movementControllerFactory;
        private readonly MovementComponent.Factory _movementComponentFactory;
        private readonly SensorySystem.Factory _sensorySystemFactory;
        private readonly ScreenSizeController _screenSizeController;

        private readonly List<IEntityPoolItem<IMonoEntity>> _novatarPool;

        public NovatarSpawner(
            NovatarSpawnerConfig novatarSpawnerConfig,
            NovatarConfig novatarConfig,
            NovatarFacade.Factory novatarFacadeFactory,
            NovatarEntity.Factory novatarEntityFactory,
            NovatarStateModel.Factory novatarStateModelFactory,
            NovatarBehaviourTree.Factory novatarBehaviourTreeFactory,
            MovementModel.Factory movementModelFactory,
            MovementController.Factory movementControllerFactory,
            MovementComponent.Factory movementComponentFactory,
            SensorySystem.Factory sensorySystemFactory,
            ScreenSizeController screenSizeController)
        {
            _novatarSpawnerConfig = novatarSpawnerConfig;
            _novatarConfig = novatarConfig;
            _novatarFacadeFactory = novatarFacadeFactory;
            _novatarEntityFactory = novatarEntityFactory;
            _novatarStateModelFactory = novatarStateModelFactory;
            _novatarBehaviourTreeFactory = novatarBehaviourTreeFactory;
            _movementModelFactory = movementModelFactory;
            _movementControllerFactory = movementControllerFactory;
            _movementComponentFactory = movementComponentFactory;
            _sensorySystemFactory = sensorySystemFactory;
            _screenSizeController = screenSizeController;

            _novatarPool = new List<IEntityPoolItem<IMonoEntity>>();

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

        private IEntityPoolItem<IMonoEntity> GetFreeEntity()
        {
            var freeItem = _novatarPool.FirstOrDefault(item => item.IsFree);
            if (freeItem != null)
            {
                return freeItem;
            }

            return CreateEntity();
        }

        private IEntityPoolItem<IMonoEntity> CreateEntity()
        {
            var novatarEntity = _novatarEntityFactory.Create(
                _novatarConfig.NovatarPrefab);

            var novatarStateModel = _novatarStateModelFactory
                .Create()
                .AddTo(Disposer);

            var novatarFacade = _novatarFacadeFactory.Create(
                    novatarEntity,
                    novatarStateModel)
                .AddTo(Disposer);

            var sensorySystem = _sensorySystemFactory
                .Create(novatarFacade, novatarStateModel)
                .AddTo(Disposer);
            sensorySystem.Initialize();

            var novatarMovementModel = _movementModelFactory
                .Create(_novatarConfig.MovementConfig)
                .AddTo(Disposer);

            var novatarMovementController = _movementControllerFactory
                .Create(novatarMovementModel, novatarFacade)
                .AddTo(Disposer);

            _movementComponentFactory
                .Create(novatarFacade, novatarMovementModel)
                .AddTo(Disposer);

            _novatarBehaviourTreeFactory
                .Create(novatarFacade, novatarStateModel, sensorySystem, novatarMovementController)
                .AddTo(Disposer)
                .Initialize();

            _novatarPool.Add(novatarFacade);
            novatarEntity.gameObject.name = $"{nameof(novatarEntity)} [{_novatarPool.Count - 1}]";

            return novatarFacade;
        }

        private Vector3 GetSpawnPosition(IMonoEntity entity)
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
