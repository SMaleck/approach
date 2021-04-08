using System.Collections.Generic;
using System.Linq;
using _Source.Services.SavegameSystem.Models;

namespace _Source.Features.Tutorials.Savegame
{
    public class TutorialsCollectionSavegame : AbstractSavegame
    {
        public IReadOnlyDictionary<TutorialId, TutorialSavegame> Savegames;

        public TutorialsCollectionSavegame(
            TutorialsCollectionSavegameData savegameData,
            TutorialSavegame.Factory savegameFactory)
        {
            Savegames = savegameData.TutorialSavegames.ToDictionary(
                e => e.Id,
                savegameFactory.Create);
        }
    }
}
