using _Source.Util;
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

        private readonly Subject<Unit> _onReset;
        public IOptimizedObservable<Unit> OnReset => _onReset;

        private readonly Subject<Unit> _onResetIdleTimeouts;
        public IOptimizedObservable<Unit> OnResetIdleTimeouts => _onResetIdleTimeouts;


        public NovatarStateModel()
        {
            _isAlive = new ReactiveProperty<bool>().AddTo(Disposer);
            _spawnPosition = new ReactiveProperty<Vector3>().AddTo(Disposer);
            _currentEntityState = new ReactiveProperty<EntityState>().AddTo(Disposer);

            _onReset = new Subject<Unit>().AddTo(Disposer);
            _onResetIdleTimeouts = new Subject<Unit>().AddTo(Disposer);
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

        public void PublishOnReset()
        {
            _onReset.OnNext(Unit.Default);
        }

        public void PublishOnResetIdleTimeouts()
        {
            _onResetIdleTimeouts.OnNext(Unit.Default);
        }
    }
}
