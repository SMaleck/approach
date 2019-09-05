using UniRx;

namespace _Source.Features.SceneManagement
{
    public interface ISceneManagementModel
    {
        IReadOnlyReactiveProperty<Scenes> CurrentScene { get; }
        IOptimizedObservable<Unit> OnSceneStarted { get; }
    }
}
