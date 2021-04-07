using _Source.Features.PlayerStatistics.Savegame;
using _Source.Services.SavegameSystem.Models;
using Packages.SavegameSystem;
using Packages.SavegameSystem.Models;

namespace _Source.Services.SavegameSystem
{
    public class SavegameFactory : ISavegameFactory
    {
        public ISavegameData Create()
        {
            return new SavegameData()
            {
                PlayerStatisticsSavegameData = new PlayerStatisticsSavegameData()
            };
        }
    }
}
