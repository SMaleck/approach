using _Source.Features.GameRound;
using _Source.Util;
using UniRx;

namespace _Source.Features.PlayerStatistics
{
    public class PlayerStatisticsController : AbstractDisposableFeature
    {
        private readonly IPlayerStatisticsModel _model;
        private readonly IGameRoundStateModel _gameRoundStateModel;

        public PlayerStatisticsController(
            IPlayerStatisticsModel model,
            IGameRoundStateModel gameRoundStateModel)
        {
            _model = model;
            _gameRoundStateModel = gameRoundStateModel;

            _gameRoundStateModel.OnRoundEnded
                .Subscribe(_ => OnRoundEnded())
                .AddTo(Disposer);
        }

        public void OnRoundEnded()
        {
            _model.IncrementRoundsPlayed();
        }
    }
}
