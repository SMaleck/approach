using _Source.Util;
using Assets._Source.Entities.Novatar;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Novatar
{
    public class NovatarStateModel : AbstractDisposable, INovatarStateModel
    {
        public class Factory : PlaceholderFactory<NovatarStateModel> { }

        private readonly ReactiveProperty<EntityState> _currentEntityState;
        public IReadOnlyReactiveProperty<EntityState> CurrentEntityState => _currentEntityState;

        private readonly ReactiveProperty<bool> _isAlive;
        public IReadOnlyReactiveProperty<bool> IsAlive => _isAlive;

        private readonly ReactiveProperty<Vector3> _spawnPosition;
        public IReadOnlyReactiveProperty<Vector3> SpawnPosition => _spawnPosition;

        private readonly ReactiveProperty<float> _currentDistanceToAvatar;
        public IReadOnlyReactiveProperty<float> CurrentDistanceToAvatar => _currentDistanceToAvatar;        

        private readonly Subject<Unit> _onReset;
        public IOptimizedObservable<Unit> OnReset => _onReset;

        public NovatarStateModel()
        {
            _isAlive = new ReactiveProperty<bool>().AddTo(Disposer);
            _spawnPosition = new ReactiveProperty<Vector3>().AddTo(Disposer);
            _currentEntityState = new ReactiveProperty<EntityState>().AddTo(Disposer);
            _currentDistanceToAvatar = new ReactiveProperty<float>().AddTo(Disposer);            
            _onReset = new Subject<Unit>().AddTo(Disposer);
        }

        public void SetCurrentEntityState(EntityState value)
        {
            _currentEntityState.Value = value;
        }

        public void SetIsAlive(bool value)
        {
            _isAlive.Value = value;
        }

        public void SetSpawnPosition(Vector3 value)
        {
            _spawnPosition.Value = value;
        }        

        public void SetCurrentDistanceToAvatar(float value)
        {
            _currentDistanceToAvatar.Value = value;
        }

        public void PublishOnReset()
        {
            _onReset.OnNext(Unit.Default);
        }
    }
}
