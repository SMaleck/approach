using _Source.Entities.Novatar;
using _Source.Features.GameWorld.Data;
using _Source.Features.NovatarBehaviour;
using _Source.Util;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace _Source.Features.GameWorld
{
    public class NovatarSpawner : AbstractDisposable
    {
        private readonly NovatarSpawnerConfig _novatarSpawnerConfig;
        private readonly NovatarEntity.Factory _novatarFactory;
        private readonly NovatarStateModel.Factory _novatarStateModelFactory;
        private readonly NovatarConfig _novatarConfig;
        private readonly NovatarBehaviourTree.Factory _novatarBehaviourTreeFactory;
        private readonly ScreenSizeController _screenSizeController;

        private readonly List<NovatarEntity> _novatarPool;

        public NovatarSpawner(
            NovatarSpawnerConfig novatarSpawnerConfig,
            NovatarConfig novatarConfig,
            NovatarEntity.Factory novatarFactory,
            NovatarStateModel.Factory novatarStateModelFactory,
            NovatarBehaviourTree.Factory novatarBehaviourTreeFactory,
            ScreenSizeController screenSizeController)
        {
            _novatarSpawnerConfig = novatarSpawnerConfig;
            _novatarConfig = novatarConfig;
            _novatarFactory = novatarFactory;
            _novatarStateModelFactory = novatarStateModelFactory;
            _novatarBehaviourTreeFactory = novatarBehaviourTreeFactory;
            _screenSizeController = screenSizeController;

            _novatarPool = new List<NovatarEntity>();

            Observable.Interval(TimeSpan.FromSeconds(_novatarSpawnerConfig.SpawnIntervalSeconds))
                .Subscribe(_ => OnInterval())
                .AddTo(Disposer);
        }

        private void OnInterval()
        {
            if (_novatarPool.Count >= _novatarSpawnerConfig.MaxActiveSpawns)
            {
                return;
            }

            var novatar = _novatarFactory.Create(
                _novatarConfig.NovatarPrefab);

            var novatarStateModel = _novatarStateModelFactory
                .Create()
                .AddTo(Disposer);

            var spawnPosition = GetSpawnPosition(novatar);

            novatar.Setup(
                novatarStateModel,
                spawnPosition);

            _novatarBehaviourTreeFactory
                .Create(novatar, novatarStateModel)
                .AddTo(Disposer)
                .Initialize();

            novatarStateModel.IsAlive
                .Subscribe(novatar.SetActive)
                .AddTo(Disposer);

            novatarStateModel.SetIsAlive(true);

            _novatarPool.Add(novatar);
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
