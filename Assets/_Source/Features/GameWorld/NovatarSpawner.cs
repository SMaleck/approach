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
        private readonly ScreenSizeModel _screenSizeModel;

        private readonly List<NovatarEntity> _novatarPool;

        public NovatarSpawner(
            NovatarSpawnerConfig novatarSpawnerConfig,
            NovatarConfig novatarConfig,
            NovatarEntity.Factory novatarFactory,
            NovatarStateModel.Factory novatarStateModelFactory,
            NovatarBehaviourTree.Factory novatarBehaviourTreeFactory,
            ScreenSizeModel screenSizeModel)
        {
            _novatarSpawnerConfig = novatarSpawnerConfig;
            _novatarConfig = novatarConfig;
            _novatarFactory = novatarFactory;
            _novatarStateModelFactory = novatarStateModelFactory;
            _novatarBehaviourTreeFactory = novatarBehaviourTreeFactory;
            _screenSizeModel = screenSizeModel;

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

            var novatarStateModel = _novatarStateModelFactory
                .Create()
                .AddTo(Disposer);

            var novatar = _novatarFactory.Create(
                _novatarConfig.NovatarPrefab);

            novatar.SetupWithModel(novatarStateModel);

            var spawnPosition = GetSpawnPosition(novatar);
            novatar.SetPosition(spawnPosition);

            _novatarBehaviourTreeFactory
                .Create(novatar, novatarStateModel)
                .AddTo(Disposer)
                .Initialize();

            _novatarPool.Add(novatar);
        }

        private Vector3 GetSpawnPosition(NovatarEntity novatar)
        {
            var spawnSideInt = UnityEngine.Random.Range(0, 4);
            var spawnSide = (ScreenSide)spawnSideInt;

            var novatarHalfSize = GetHalfSizeFor(spawnSide, novatar.Size);
            var randomComponent = GetRandomComponentFor(spawnSide, novatar.Size);

            switch (spawnSide)
            {
                case ScreenSide.Top:
                    return new Vector3(randomComponent, _screenSizeModel.HeightExtendUnits + novatarHalfSize, 0);

                case ScreenSide.Bottom:
                    return new Vector3(randomComponent, -(_screenSizeModel.HeightExtendUnits + novatarHalfSize), 0);

                case ScreenSide.Left:
                    return new Vector3(-(_screenSizeModel.WidthExtendUnits + novatarHalfSize), randomComponent, 0);

                case ScreenSide.Right:
                    return new Vector3(_screenSizeModel.WidthExtendUnits + novatarHalfSize, randomComponent, 0);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float GetHalfSizeFor(ScreenSide spawnSide, Vector2 entitySize)
        {
            switch (spawnSide)
            {
                case ScreenSide.Top:
                case ScreenSide.Bottom:
                    return entitySize.y / 2;

                case ScreenSide.Left:
                case ScreenSide.Right:
                    return entitySize.x / 2;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float GetRandomComponentFor(ScreenSide spawnSide, Vector2 entitySize)
        {

            switch (spawnSide)
            {
                case ScreenSide.Top:
                case ScreenSide.Bottom:
                    var maxWidthExtend = _screenSizeModel.WidthExtendUnits - (entitySize.x / 2);
                    return UnityEngine.Random.Range(-maxWidthExtend, maxWidthExtend);

                case ScreenSide.Left:
                case ScreenSide.Right:
                    var maxHeightExtend = _screenSizeModel.HeightExtendUnits - (entitySize.y / 2);
                    return UnityEngine.Random.Range(-maxHeightExtend, maxHeightExtend);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
