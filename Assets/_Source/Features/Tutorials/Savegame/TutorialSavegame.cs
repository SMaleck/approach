using _Source.Services.SavegameSystem.Models;
using UniRx;
using Zenject;

namespace _Source.Features.Tutorials.Savegame
{
    public class TutorialSavegame : AbstractSavegame
    {
        public class Factory : PlaceholderFactory<TutorialSavegameData, TutorialSavegame> { }

        private readonly TutorialSavegameData _savegameData;

        public TutorialId Id => _savegameData.Id;
        public IReactiveProperty<bool> IsCompleted { get; }

        public TutorialSavegame(TutorialSavegameData savegameData)
        {
            _savegameData = savegameData;

            IsCompleted = CreateBoundProperty(
                savegameData.IsCompleted,
                value => savegameData.IsCompleted = value);
        }
    }
}
