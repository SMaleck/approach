using _Source.Features.NovatarBehaviour;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Novatar
{
    public class NovatarStateModel : AbstractDisposable
    {
        public class Factory : PlaceholderFactory<NovatarStateModel> { }

        private readonly ReactiveProperty<bool> _isAlive;
        public IReadOnlyReactiveProperty<bool> IsAlive => _isAlive;

        private readonly ReactiveProperty<Vector3> _spawnPosition;
        public IReadOnlyReactiveProperty<Vector3> SpawnPosition => _spawnPosition;

        private readonly ReactiveProperty<RelationshipStatus> _currentRelationshipStatus;
        public IReadOnlyReactiveProperty<RelationshipStatus> CurrentRelationshipStatus => _currentRelationshipStatus;

        private readonly ReactiveProperty<float> _currentDistanceToAvatar;
        public IReadOnlyReactiveProperty<float> CurrentDistanceToAvatar => _currentDistanceToAvatar;

        private readonly ReactiveProperty<double> _timePassedInCurrentStatusSeconds;
        public IReadOnlyReactiveProperty<double> TimePassedInCurrentStatusSeconds => _timePassedInCurrentStatusSeconds;

        private readonly Subject<Unit> _onReset;
        public IOptimizedObservable<Unit> OnReset => _onReset;

        public NovatarStateModel()
        {
            _isAlive = new ReactiveProperty<bool>().AddTo(Disposer);
            _spawnPosition = new ReactiveProperty<Vector3>().AddTo(Disposer);
            _currentRelationshipStatus = new ReactiveProperty<RelationshipStatus>().AddTo(Disposer);
            _currentDistanceToAvatar = new ReactiveProperty<float>().AddTo(Disposer);
            _timePassedInCurrentStatusSeconds = new ReactiveProperty<double>().AddTo(Disposer);
            _onReset = new Subject<Unit>().AddTo(Disposer);
        }

        public void SetIsAlive(bool value)
        {
            _isAlive.Value = value;
        }

        public void SetSpawnPosition(Vector3 value)
        {
            _spawnPosition.Value = value;
        }

        public void SetCurrentRelationshipStatus(RelationshipStatus value)
        {
            _currentRelationshipStatus.Value = value;
        }

        public void SetCurrentDistanceToAvatar(float value)
        {
            _currentDistanceToAvatar.Value = value;
        }

        public void SetTimePassedInCurrentStatusSeconds(double value)
        {
            _timePassedInCurrentStatusSeconds.Value = value;
        }

        public void PublishOnReset()
        {
            _onReset.OnNext(Unit.Default);
        }
    }
}
