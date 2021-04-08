using _Source.Features.PlayerStatistics.Savegame;
using _Source.Features.Tutorials.Savegame;
using Packages.SavegameSystem.Models;
using System;

namespace _Source.Services.SavegameSystem.Models
{
    [Serializable]
    public class SavegameData : ISavegameData
    {
        public PlayerStatisticsSavegameData PlayerStatisticsSavegameData;
        public TutorialsCollectionSavegameData TutorialsCollectionSavegameData;
    }
}
