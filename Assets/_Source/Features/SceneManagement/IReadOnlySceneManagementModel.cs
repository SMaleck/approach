using UniRx;

namespace _Source.Features.SceneManagement
{
    public interface IReadOnlySceneManagementModel
    {
        IReadOnlyReactiveProperty<Scenes> CurrentScene { get; }
        IOptimizedObservable<Unit> OnSceneStarted { get; }
    }
}
