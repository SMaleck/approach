using Packages.SavegameSystem.Models;
using System;
using System.Collections.Generic;

namespace _Source.Features.Tutorials.Savegame
{
    [Serializable]
    public class TutorialsCollectionSavegameData : ISavegameData
    {
        public List<TutorialSavegameData> TutorialSavegames;
    }
}
