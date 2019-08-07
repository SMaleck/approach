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

        public NovatarStateModel()
        {
            _isAlive = new ReactiveProperty<bool>().AddTo(Disposer);
            _currentRelationshipStatus = new ReactiveProperty<RelationshipStatus>().AddTo(Disposer);
            _currentDistanceToAvatar = new ReactiveProperty<float>().AddTo(Disposer);
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
    }
}
