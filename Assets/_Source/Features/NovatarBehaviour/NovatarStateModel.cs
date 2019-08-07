using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.NovatarBehaviour
{
    public class NovatarStateModel : AbstractDisposable
    {
        public class Factory : PlaceholderFactory<NovatarStateModel> { }

        private readonly ReactiveProperty<RelationshipStatus> _currentRelationshipStatus;
        public IReadOnlyReactiveProperty<RelationshipStatus> CurrentRelationshipStatus => _currentRelationshipStatus;

        private readonly ReactiveProperty<float> _currentDistanceToAvatar;
        public IReadOnlyReactiveProperty<float> CurrentDistanceToAvatar => _currentDistanceToAvatar;

        public NovatarStateModel()
        {
            _currentRelationshipStatus = new ReactiveProperty<RelationshipStatus>().AddTo(Disposer);
            _currentDistanceToAvatar = new ReactiveProperty<float>().AddTo(Disposer);
        }

        public void SetCurrentStatus(RelationshipStatus value)
        {
            _currentRelationshipStatus.Value = value;
        }

        public void SetCurrentDistanceToAvatar(float value)
        {
            _currentDistanceToAvatar.Value = value;
        }
    }
}
