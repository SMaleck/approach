namespace _Source.Features.Tutorials
{
    public interface ITutorialsCollectionModel
    {
        ITutorialModel this[TutorialId id] { get; }
    }
}