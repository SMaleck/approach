using _Source.Services.SavegameSystem.Models;
using UniRx;
using Zenject;

namespace _Source.Features.Tutorials.Savegame
{
    public class TutorialSavegame : AbstractSavegame, ITutorialSavegame
    {
        public class Factory : PlaceholderFactory<TutorialSavegameData, TutorialSavegame> { }

        private readonly TutorialSavegameData _savegameData;

        public TutorialId Id => _savegameData.Id;
        public IReactiveProperty<TutorialState> State { get; }

        public TutorialSavegame(TutorialSavegameData savegameData)
        {
            _savegameData = savegameData;

            State = CreateBoundProperty(
                savegameData.State,
                value => savegameData.State = value);
        }
    }
}
