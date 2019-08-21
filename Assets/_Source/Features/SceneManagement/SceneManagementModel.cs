using _Source.Util;
using UniRx;

namespace _Source.Features.SceneManagement
{
    public class SceneManagementModel : AbstractDisposable, IReadOnlySceneManagementModel
    {
        private readonly ReactiveProperty<bool> _isLoadingScreenVisible;
        public IReadOnlyReactiveProperty<bool> IsLoadingScreenVisible => _isLoadingScreenVisible;

        private readonly Subject<Unit> _onOpenLoadingScreenCompleted;
        public IOptimizedObservable<Unit> OnOpenLoadingScreenCompleted => _onOpenLoadingScreenCompleted;

        private readonly Subject<Unit> _onSceneStarted;
        public IOptimizedObservable<Unit> OnSceneStarted => _onSceneStarted;

        public SceneManagementModel()
        {
            _isLoadingScreenVisible = new ReactiveProperty<bool>().AddTo(Disposer);
            _onOpenLoadingScreenCompleted = new Subject<Unit>().AddTo(Disposer);
        }

        public void SetIsLoadingScreenVisible(bool value)
        {
            _isLoadingScreenVisible.Value = value;
        }

        public void PublishOnOpenLoadingScreenCompleted()
        {
            _onOpenLoadingScreenCompleted.OnNext(Unit.Default);
        }

        public void PublishOnSceneStarted()
        {
            _onSceneStarted.OnNext(Unit.Default);
        }
    }
}
