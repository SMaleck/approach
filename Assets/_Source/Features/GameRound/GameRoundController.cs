using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.GameRound
{
    // ToDo V1 Make this depend on a max time as well
    // ToDo V1 Track amount of Relationships and show result
    public class GameRoundController : AbstractDisposable, IInitializable
    {
        private readonly GameRoundModel _gameRoundModel;
        private readonly IAvatarLocator _avatarLocator;

        public GameRoundController(
            GameRoundModel gameRoundModel,
            IAvatarLocator avatarLocator)
        {
            _gameRoundModel = gameRoundModel;
            _avatarLocator = avatarLocator;
        }

        public void Initialize()
        {
            var healthDataComponent = _avatarLocator.AvatarActorStateModel.Get<HealthDataComponent>();
            healthDataComponent.IsAlive
                .Where(isAlive => !isAlive)
                .Take(1)
                .Subscribe(_ => _gameRoundModel.PublishOnRoundEnded())
                .AddTo(Disposer);
        }

        public void PauseRound(bool isPaused)
        {
            _gameRoundModel.SetIsPaused(isPaused);
        }
    }
}
