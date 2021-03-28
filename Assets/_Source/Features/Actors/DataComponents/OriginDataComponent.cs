using _Source.Features.ScreenSize;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class OriginDataComponent : AbstractDataComponent
    {
        public class Factory : PlaceholderFactory<OriginDataComponent> { }

        private readonly ScreenSizeModel _screenSizeModel;

        private readonly ReactiveProperty<Vector3> _spawnPosition;
        public IReadOnlyReactiveProperty<Vector3> SpawnPosition => _spawnPosition;

        public ScreenEdge SpawnEdge { get; private set; }

        public OriginDataComponent(ScreenSizeModel screenSizeModel)
        {
            _spawnPosition = new ReactiveProperty<Vector3>().AddTo(Disposer);
            _screenSizeModel = screenSizeModel;
        }

        public void SetSpawnPosition(Vector3 value)
        {
            _spawnPosition.Value = value;
            SpawnEdge = _screenSizeModel.OutsideVectorToScreenEdge(value);
        }
    }
}
