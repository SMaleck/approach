using _Source.Features.PlayerStatistics.Savegame;
using _Source.Features.Tutorials.Savegame;
using _Source.Services.SavegameSystem.Models;
using _Source.Util;
using Packages.SavegameSystem;
using Zenject;

namespace _Source.Services.SavegameSystem.Installation
{
    public class SavegameInstaller : Installer<SavegameInstaller>
    {
        [Inject] private readonly ISavegameService _savegameService;

        public override void InstallBindings()
        {
            var savegameData = _savegameService.Load() as SavegameData;

            BindSavegameFactories();

            Container.BindSingleSavegame<PlayerStatisticsSavegame, PlayerStatisticsSavegameData>(
                savegameData.PlayerStatisticsSavegameData);
            
            Container.BindSingleSavegame<TutorialsCollectionSavegame, TutorialsCollectionSavegameData>(
                savegameData.TutorialsCollectionSavegameData);
        }

        private void BindSavegameFactories()
        {
            Container.BindFactory<TutorialSavegameData, TutorialSavegame, TutorialSavegame.Factory>()
                .AsSingle();
        }
    }
}
