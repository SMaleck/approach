using _Source.Features.ViewManagement;
using UniRx;

namespace _Source.Util
{
    public class AbstractView : AbstractDisposableMonoBehaviour, IClosableView
    {
        private Subject<Unit> _onOpened;
        protected Subject<Unit> OnOpened => _onOpened ?? (_onOpened = new Subject<Unit>().AddTo(Disposer));

        private Subject<Unit> _onClosed;
        protected Subject<Unit> OnClosed => _onClosed ?? (_onClosed = new Subject<Unit>().AddTo(Disposer));

        public void Open()
        {
            SetActive(true);
            OnOpened.OnNext(Unit.Default);
        }

        public void Close()
        {
            SetActive(false);
            OnClosed.OnNext(Unit.Default);
        }
    }
}
