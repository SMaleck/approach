using UniRx;

namespace _Source.Features.Tutorials.Savegame
{
    public interface ITutorialSavegame
    {
        TutorialId Id { get; }
        IReactiveProperty<TutorialState> State { get; }
    }
}