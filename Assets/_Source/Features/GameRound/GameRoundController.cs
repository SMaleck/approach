using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using UniRx;

namespace _Source.Features.GameRound
{
    // ToDo V1 Make this depend on a max time as well
    // ToDo V1 Track amount of Relationships and show result
    public class GameRoundController : AbstractDisposable
    {
        private readonly GameRoundModel _gameRoundModel;

        public GameRoundController(
            GameRoundModel gameRoundModel,
            IActorStateModel actorStateModel)
        {
            _gameRoundModel = gameRoundModel;

            var healthDataComponent = actorStateModel.Get<HealthDataComponent>();
            healthDataComponent.IsAlive
                .Where(isAlive => !isAlive)
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
