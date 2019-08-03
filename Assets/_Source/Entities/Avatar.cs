using _Source.Features.UserInput;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Entities
{
    public class Avatar : AbstractMonoEntity, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Avatar> { }

        private AvatarConfig _avatarConfig;
        private IReadOnlyUserInputModel _userInputModel;

        [Inject]
        private void Inject(
            AvatarConfig avatarConfig,
            IReadOnlyUserInputModel userInputModel)
        {
            _avatarConfig = avatarConfig;
            _userInputModel = userInputModel;
        }

        public void Initialize()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (!_userInputModel.HasInput)
            {
                return;
            }

            var timeAdjustedSpeed = _avatarConfig.Speed.AsTimeAdjusted();
            var translateTarget = _userInputModel.InputVector.Value * timeAdjustedSpeed;

            transform.Translate(translateTarget);
        }
    }
}
