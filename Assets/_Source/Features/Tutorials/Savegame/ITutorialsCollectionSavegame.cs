using System.Collections.Generic;

namespace _Source.Features.Tutorials.Savegame
{
    public interface ITutorialsCollectionSavegame
    {
        IReadOnlyDictionary<TutorialId, ITutorialSavegame> Savegames { get; }
    }
}