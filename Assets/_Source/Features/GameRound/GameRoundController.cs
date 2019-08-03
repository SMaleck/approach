using _Source.Features.SurvivalStats;
using _Source.Util;
using UniRx;

namespace _Source.Features.GameRound
{
    public class GameRoundController : AbstractDisposable
    {
        public GameRoundController(
            GameRoundModel gameRoundModel,
            SurvivalStatsModel survivalStatsModel)
        {
            survivalStatsModel.IsAlive
                .Where(isAlive => isAlive)
                .Take(1)
                .Subscribe(_ => gameRoundModel.PublishOnRoundEnded())
                .AddTo(Disposer);
        }
    }
}
