using _Source.Features.ActorBehaviours;
using _Source.Features.ActorEntities.Novatar.Config;
using _Source.Features.Actors.Creation;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using _Source.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    public class NovatarSpawner : AbstractDisposable
    {
        [Inject] private readonly INovatarActorFactory _novatarActorActorFactory;
        [Inject] private readonly MonoEntity.Factory _entityFactory;
        [Inject] private readonly NovatarFacade.Factory _novatarFacadeFactory;
        [Inject] private readonly NovatarBehaviourTree.Factory _novatarBehaviourTreeFactory;
        [Inject] private readonly MovementController.Factory _movementControllerFactory;

        private readonly NovatarSpawnerConfig _novatarSpawnerConfig;
        private readonly NovatarConfig _novatarConfig;
        private readonly ScreenSizeController _screenSizeController;

        private readonly List<IEntityPoolItem<IMonoEntity>> _novatarPool;

        public NovatarSpawner(
            NovatarSpawnerConfig novatarSpawnerConfig,
            NovatarConfig novatarConfig,
            ScreenSizeController screenSizeController)
        {
            _novatarSpawnerConfig = novatarSpawnerConfig;
            _novatarConfig = novatarConfig;
            _screenSizeController = screenSizeController;

            _novatarPool = new List<IEntityPoolItem<IMonoEntity>>();
        }

        public void Spawn()
        {
            var novatarPoolItem = GetFreeEntity();

            var spawnPosition = GetSpawnPosition(novatarPoolItem.Entity);
            novatarPoolItem.Reset(spawnPosition);
        }

        public int GetActiveNovatarCount()
        {
            return _novatarPool.Count(item => !item.IsFree);
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
            var novatarEntity = _entityFactory.Create(
                _novatarConfig.NovatarPrefab);

            var actorStateModel = _novatarActorActorFactory.CreateNovatar();

            var novatarFacade = _novatarFacadeFactory.Create(
                novatarEntity,
                actorStateModel);

            var movementController = _movementControllerFactory
                .Create(actorStateModel);

            _novatarBehaviourTreeFactory
                .Create(actorStateModel, movementController)
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
