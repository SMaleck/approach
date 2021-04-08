using UniRx;

namespace _Source.Features.Tutorials
{
    public interface ITutorialModel
    {
        TutorialId Id { get; }
        IReadOnlyReactiveProperty<bool> IsCompleted { get; }
        void Complete();
    }
}