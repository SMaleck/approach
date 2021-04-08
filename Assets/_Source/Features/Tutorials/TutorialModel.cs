using _Source.Features.Tutorials.Savegame;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.Tutorials
{
    public class TutorialModel : AbstractDisposableFeature, ITutorialModel
    {
        public class Factory : PlaceholderFactory<ITutorialSavegame, TutorialModel> { }

        private readonly ITutorialSavegame _savegame;

        public TutorialId Id => _savegame.Id;
        public IReadOnlyReactiveProperty<TutorialState> State => _savegame.State;

        public TutorialModel(ITutorialSavegame savegame)
        {
            _savegame = savegame;
        }

        public void Start()
        {
            SetState(TutorialState.Running);
        }

        public void Complete()
        {
            SetState(TutorialState.Completed);
        }

        private void SetState(TutorialState state)
        {
            if (State.Value < state)
            {
                _savegame.State.Value = state;
            }
        }
    }
}
