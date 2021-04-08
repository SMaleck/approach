using _Source.Features.PlayerStatistics.Savegame;
using _Source.Features.Tutorials;
using _Source.Features.Tutorials.Savegame;
using _Source.Services.SavegameSystem.Models;
using _Source.Util;
using Packages.SavegameSystem;
using Packages.SavegameSystem.Models;
using System.Linq;

namespace _Source.Services.SavegameSystem
{
    public class SavegameFactory : ISavegameFactory
    {
        public ISavegameData Create()
        {
            return new SavegameData()
            {
                PlayerStatisticsSavegameData = CreatePlayerStatisticsSavegameData(),
                TutorialsCollectionSavegameData = CreateTutorialsCollectionSavegameData()
            };
        }

        private PlayerStatisticsSavegameData CreatePlayerStatisticsSavegameData()
        {
            return new PlayerStatisticsSavegameData();
        }

        private TutorialsCollectionSavegameData CreateTutorialsCollectionSavegameData()
        {
            var savegames = EnumHelper<TutorialId>.Iterator
                .Select(CreateTutorialSavegameData)
                .ToList();

            return new TutorialsCollectionSavegameData
            {
                TutorialSavegames = savegames
            };
        }

        private TutorialSavegameData CreateTutorialSavegameData(TutorialId id)
        {
            return new TutorialSavegameData
            {
                Id = id,
                IsCompleted = false
            };
        }
    }
}
