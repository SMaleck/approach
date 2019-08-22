using _Source.Util;
using UniRx;

namespace _Source.Features.SceneManagement
{
    public class LoadingScreenModel : AbstractDisposable
    {
        private readonly ReactiveProperty<bool> _isLoadingScreenVisible;
        public IReadOnlyReactiveProperty<bool> IsLoadingScreenVisible => _isLoadingScreenVisible;

        private readonly Subject<Unit> _onOpenLoadingScreenCompleted;
        public IOptimizedObservable<Unit> OnOpenLoadingScreenCompleted => _onOpenLoadingScreenCompleted;

        private readonly Subject<Unit> _onCloseLoadingScreenCompleted;
        public IOptimizedObservable<Unit> OnCloseLoadingScreenCompleted => _onCloseLoadingScreenCompleted;

        public LoadingScreenModel()
        {
            _isLoadingScreenVisible = new ReactiveProperty<bool>().AddTo(Disposer);
            _onOpenLoadingScreenCompleted = new Subject<Unit>().AddTo(Disposer);
            _onCloseLoadingScreenCompleted = new Subject<Unit>().AddTo(Disposer);
        }

        public void SetIsLoadingScreenVisible(bool value)
        {
            _isLoadingScreenVisible.Value = value;
        }

        public void PublishOnOpenLoadingScreenCompleted()
        {
            _onOpenLoadingScreenCompleted.OnNext(Unit.Default);
        }

        public void PublishOnCloseLoadingScreenCompleted()
        {
            _onCloseLoadingScreenCompleted.OnNext(Unit.Default);
        }
    }
}
