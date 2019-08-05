using _Source.Entities;
using _Source.Util;
using System;
using UniRx;
using UnityEngine;

namespace _Source.Features.GameWorld
{
    public class NovatarSpawner : AbstractDisposable
    {
        private readonly Novatar.Factory _novatarFactory;
        private readonly NovatarConfig _novatarConfig;
        private readonly ScreenSizeModel _screenSizeModel;

        public NovatarSpawner(
            Novatar.Factory novatarFactory,
            NovatarConfig novatarConfig,
            ScreenSizeModel screenSizeModel)
        {
            _novatarFactory = novatarFactory;
            _novatarConfig = novatarConfig;
            _screenSizeModel = screenSizeModel;

            Observable.Interval(TimeSpan.FromSeconds(_novatarConfig.SpawnIntervalSeconds))
                .Subscribe(_ => OnInterval())
                .AddTo(Disposer);
        }

        private void OnInterval()
        {
            var novatar = _novatarFactory.Create(
                _novatarConfig.NovatarPrefab);

            var spawnPosition = GetSpawnPosition(novatar);
            novatar.SetPosition(spawnPosition);

            novatar.Initialize();
        }

        private Vector3 GetSpawnPosition(Novatar novatar)
        {
            var spawnSideInt = UnityEngine.Random.Range(0, 4);
            var spawnSide = (ScreenSide)spawnSideInt;

            var novatarHalfSize = GetHalfSizeFor(spawnSide, novatar.Size);
            var randomComponent = GetRandomComponentFor(spawnSide, novatar.Size);

            App.Logger.Warn($"{spawnSideInt} -> {spawnSide}");
            App.Logger.Warn($"{_screenSizeModel.WidthExtendUnits} / {_screenSizeModel.HeightExtendUnits}");

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
