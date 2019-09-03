using _Source.Entities.Avatar;
using _Source.Util;
using UniRx;

namespace _Source.Features.GameRound
{
    public class GameRoundController : AbstractDisposable
    {
        private readonly GameRoundModel _gameRoundModel;

        public GameRoundController(
            GameRoundModel gameRoundModel,
            AvatarStateModel survivalStatsModel)
        {
            _gameRoundModel = gameRoundModel;
            survivalStatsModel.IsAlive
                .Where(isAlive => isAlive)
                .Take(1)
                .Subscribe(_ => gameRoundModel.PublishOnRoundEnded())
                .AddTo(Disposer);
        }

        public void PauseRound(bool isPaused)
        {
            _gameRoundModel.SetIsPaused(isPaused);
        }
    }
}
