using _Source.Util;
using UniRx;

namespace _Source.Features.GameRound
{
    public class GameRoundModel : AbstractDisposable
    {
        private readonly Subject<Unit> _onRoundEnded;
        public IOptimizedObservable<Unit> OnRoundEnded => _onRoundEnded;

        public GameRoundModel()
        {
            _onRoundEnded = new Subject<Unit>().AddTo(Disposer);
        }

        public void PublishOnRoundEnded()
        {
            _onRoundEnded.OnNext(Unit.Default);
        }
    }
}
