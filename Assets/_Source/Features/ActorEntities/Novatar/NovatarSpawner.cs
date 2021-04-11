using _Source.Features.ActorBehaviours.Creation;
using _Source.Features.ActorEntities.Config;
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
        [Inject] private readonly AiMovementController.Factory _movementControllerFactory;
        [Inject] private readonly NovatarBehaviourTreeFactory _novatarBehaviourTreeFactory;
        [Inject] private readonly NovatarFacade.Factory _novatarFacadeFactory;

        private readonly NovatarSpawnerConfig _novatarSpawnerConfig;
        private readonly ActorEntitiesConfig _actorEntitiesConfig;
        private readonly ScreenSizeController _screenSizeController;

        private readonly List<INovatarPoolItem> _novatarPool;
        public IReadOnlyList<INovatarPoolItem> Pool => _novatarPool;

        public NovatarSpawner(
            NovatarSpawnerConfig novatarSpawnerConfig,
            ActorEntitiesConfig actorEntitiesConfig,
            ScreenSizeController screenSizeController)
        {
            _novatarSpawnerConfig = novatarSpawnerConfig;
            _actorEntitiesConfig = actorEntitiesConfig;
            _screenSizeController = screenSizeController;

            _novatarPool = new List<INovatarPoolItem>();
        }

        public void Spawn()
        {
            var novatarPoolItem = GetFreeEntity();

            var spawnPosition = GetSpawnPosition(novatarPoolItem);
            novatarPoolItem.Reset(spawnPosition);
        }

        private INovatarPoolItem GetFreeEntity()
        {
            var freeItem = _novatarPool.FirstOrDefault(item => item.IsFree);
            if (freeItem != null)
            {
                return freeItem;
            }

            return CreateEntity();
        }

        private INovatarPoolItem CreateEntity()
        {
            var novatarEntity = _entityFactory.Create(
                _actorEntitiesConfig.NovatarPrefab);

            var actorStateModel = _novatarActorActorFactory.CreateNovatar();

            var movementController = _movementControllerFactory
                .Create(actorStateModel);

            var behaviourTree = _novatarBehaviourTreeFactory
                .Create(actorStateModel, movementController);

            var novatarFacade = _novatarFacadeFactory.Create(
                novatarEntity,
                actorStateModel,
                behaviourTree);

            _novatarPool.Add(novatarFacade);
            novatarEntity.gameObject.name = $"{nameof(novatarEntity)} [{_novatarPool.Count - 1}]";

            return novatarFacade;
        }

        private Vector3 GetSpawnPosition(INovatarPoolItem poolItem)
        {
            var spawnSideInt = UnityEngine.Random.Range(0, 4);
            var spawnSide = (ScreenSide)spawnSideInt;

            return _screenSizeController.GetRandomizedOutOfBoundsPosition(
                spawnSide,
                poolItem.Size,
                _novatarSpawnerConfig.SpawnPositionOffset);
        }
    }
}
