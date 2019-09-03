using UniRx;
using Zenject;

namespace _Source.Util
{
    // ToDo Should this inherit from IDisposable?
    public abstract class AbstractDisposableMonoBehaviour : AbstractMonoBehaviour
    {
        private CompositeDisposable _disposer;
        public CompositeDisposable Disposer => _disposer
            ?? (_disposer = new CompositeDisposable().AddTo(this));

        [Inject]
        private void Inject([InjectOptional] CompositeDisposable disposer)
        {
            _disposer = disposer;
        }
    }
}