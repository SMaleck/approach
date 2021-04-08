using _Source.Features.Tutorials.Savegame;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.Tutorials
{
    public class TutorialModel : AbstractDisposableFeature, ITutorialModel
    {
        public class Factory : PlaceholderFactory<TutorialSavegame, TutorialModel> { }

        private readonly TutorialSavegame _savegame;

        public TutorialId Id => _savegame.Id;
        public IReadOnlyReactiveProperty<bool> IsCompleted => _savegame.IsCompleted;

        public TutorialModel(TutorialSavegame savegame)
        {
            _savegame = savegame;
        }

        public void Complete()
        {
            _savegame.IsCompleted.Value = true;
        }
    }
}
