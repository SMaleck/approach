using System;
using _Source.Features.PlayerStatistics.Savegame;
using Packages.SavegameSystem.Models;

namespace _Source.Services.SavegameSystem.Models
{
    [Serializable]
    public class SavegameData : ISavegameData
    {
        public PlayerStatisticsSavegameData PlayerStatisticsSavegameData;
    }
}
