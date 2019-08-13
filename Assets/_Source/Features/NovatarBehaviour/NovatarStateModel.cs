using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.NovatarBehaviour
{
    public class NovatarStateModel : AbstractDisposable
    {
        public class Factory : PlaceholderFactory<NovatarStateModel> { }

        private readonly ReactiveProperty<bool> _isAlive;
        public IReadOnlyReactiveProperty<bool> IsAlive => _isAlive;

        private readonly ReactiveProperty<RelationshipStatus> _currentRelationshipStatus;
        public IReadOnlyReactiveProperty<RelationshipStatus> CurrentRelationshipStatus => _currentRelationshipStatus;

        private readonly ReactiveProperty<float> _currentDistanceToAvatar;
        public IReadOnlyReactiveProperty<float> CurrentDistanceToAvatar => _currentDistanceToAvatar;

        private readonly ReactiveProperty<double> _timePassedInCurrentStatusSeconds;
        public IReadOnlyReactiveProperty<double> TimePassedInCurrentStatusSeconds => _timePassedInCurrentStatusSeconds;

        private readonly ReactiveProperty<double> _timePassedSinceFallingBehindSeconds;
        public IReadOnlyReactiveProperty<double> TimePassedSinceFallingBehindSeconds => _timePassedSinceFallingBehindSeconds;

        public NovatarStateModel()
        {
            _isAlive = new ReactiveProperty<bool>().AddTo(Disposer);
            _currentRelationshipStatus = new ReactiveProperty<RelationshipStatus>().AddTo(Disposer);
            _currentDistanceToAvatar = new ReactiveProperty<float>().AddTo(Disposer);
            _timePassedInCurrentStatusSeconds = new ReactiveProperty<double>().AddTo(Disposer);
            _timePassedSinceFallingBehindSeconds = new ReactiveProperty<double>().AddTo(Disposer);
        }

        public void SetIsAlive(bool value)
        {
            _isAlive.Value = value;
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

        public void SetTimePassedSinceFallingBehindSeconds(double value)
        {
            _timePassedSinceFallingBehindSeconds.Value = value;
        }
    }
}
