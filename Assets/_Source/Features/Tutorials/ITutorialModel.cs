using UniRx;

namespace _Source.Features.Tutorials
{
    public interface ITutorialModel
    {
        TutorialId Id { get; }
        bool IsCompleted{ get; }
        IReadOnlyReactiveProperty<TutorialState> State { get; }

        void Start();
        void Complete();
    }
}