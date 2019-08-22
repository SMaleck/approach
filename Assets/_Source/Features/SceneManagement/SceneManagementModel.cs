using _Source.Util;
using UniRx;

namespace _Source.Features.SceneManagement
{
    public class SceneManagementModel : AbstractDisposable, IReadOnlySceneManagementModel
    {
        private readonly ReactiveProperty<Scenes> _currentScene;
        public IReadOnlyReactiveProperty<Scenes> CurrentScene => _currentScene;

        private readonly Subject<Unit> _onSceneStarted;
        public IOptimizedObservable<Unit> OnSceneStarted => _onSceneStarted;

        public SceneManagementModel()
        {
            _currentScene = new ReactiveProperty<Scenes>().AddTo(Disposer);
            _onSceneStarted = new Subject<Unit>().AddTo(Disposer);
        }

        public void SetCurrentScene(Scenes value)
        {
            _currentScene.Value = value;
        }

        public void PublishOnSceneStarted()
        {
            _onSceneStarted.OnNext(Unit.Default);
        }
    }
}
