using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using Packages.SavegameSystem;
using UniRx;
using Zenject;

namespace _Source.Features.GameRound
{
    public class GameRoundStateController : AbstractDisposable, IInitializable
    {
        private readonly GameRoundStateModel _gameRoundStateModel;
        private readonly IAvatarLocator _avatarLocator;
        private readonly ISavegameService _savegameService;

        public GameRoundStateController(
            GameRoundStateModel gameRoundStateModel,
            IAvatarLocator avatarLocator,
            ISavegameService savegameService)
        {
            _gameRoundStateModel = gameRoundStateModel;
            _avatarLocator = avatarLocator;
            _savegameService = savegameService;

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

            _savegameService.Save();
        }
    }
}
