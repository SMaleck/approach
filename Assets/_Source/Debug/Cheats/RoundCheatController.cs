using _Source.Features.GameRound.Duration;
using UniRx;
using Zenject;

namespace _Source.Debug.Cheats
{
    public class RoundCheatController : AbstractCheatController, IInitializable
    {
        private readonly IGameRoundDurationModel _gameRoundDurationModel;

        public RoundCheatController(IGameRoundDurationModel gameRoundDurationModel)
        {
            _gameRoundDurationModel = gameRoundDurationModel;
        }

        public void Initialize()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            CheckInput("x", () => _gameRoundDurationModel.SetRemainingSeconds(2));
        }
    }
}
