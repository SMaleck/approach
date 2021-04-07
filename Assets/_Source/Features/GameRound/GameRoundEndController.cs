using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound.Duration;
using _Source.Util;
using System.Linq;
using UniRx;
using Zenject;

namespace _Source.Features.GameRound
{
    public class GameRoundEndController : AbstractDisposableFeature, IInitializable
    {
        private readonly IAvatarLocator _avatarLocator;
        private readonly IGameRoundDurationModel _gameRoundDurationModel;
        private readonly GameRoundStateController _gameRoundStateController;

        public GameRoundEndController(
            IAvatarLocator avatarLocator,
            IGameRoundDurationModel gameRoundDurationModel,
            GameRoundStateController gameRoundStateController)
        {
            _avatarLocator = avatarLocator;
            _gameRoundDurationModel = gameRoundDurationModel;
            _gameRoundStateController = gameRoundStateController;
        }

        public void Initialize()
        {
            var healthDataComponent = _avatarLocator.AvatarActor.Get<HealthDataComponent>();

            Observable.Merge(
                    healthDataComponent.IsAlive,
                    _gameRoundDurationModel.IsRunning)
                .Select(_ => healthDataComponent.IsAlive.Value &&
                             _gameRoundDurationModel.IsRunning.Value)
                .Where(isRoundOngoing => !isRoundOngoing)
                .Subscribe(_ => _gameRoundStateController.EndRound())
                .AddTo(Disposer);
        }
    }
}
