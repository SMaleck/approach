using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.GameRound
{
    // ToDo V1 Track amount of Relationships and show result
    public class GameRoundStateController : AbstractDisposable, IInitializable
    {
        private readonly GameRoundStateModel _gameRoundStateModel;
        private readonly IAvatarLocator _avatarLocator;

        public GameRoundStateController(
            GameRoundStateModel gameRoundStateModel,
            IAvatarLocator avatarLocator)
        {
            _gameRoundStateModel = gameRoundStateModel;
            _avatarLocator = avatarLocator;

            PauseRound(true);
        }

        public void Initialize()
        {
            var healthDataComponent = _avatarLocator.AvatarActor.Get<HealthDataComponent>();
            healthDataComponent.IsAlive
                .Where(isAlive => !isAlive)
                .Take(1)
                .Subscribe(_ => EndRound())
                .AddTo(Disposer);
        }

        public void PauseRound(bool isPaused)
        {
            _gameRoundStateModel.SetIsPaused(isPaused);
        }

        public void StartRound()
        {
            PauseRound(false);
            _gameRoundStateModel.PublishOnRoundStarted();
        }

        public void EndRound()
        {
            PauseRound(false);
            _gameRoundStateModel.PublishOnRoundEnded();
        }
    }
}
