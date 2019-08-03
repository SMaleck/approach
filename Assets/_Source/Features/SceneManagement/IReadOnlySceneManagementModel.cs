using UniRx;

namespace _Source.Features.SceneManagement
{
    public interface IReadOnlySceneManagementModel
    {
        IOptimizedObservable<Unit> OnOpenLoadingScreenCompleted { get; }
    }
}
