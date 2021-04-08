using _Source.Features.Tutorials.Config;
using _Source.Features.Tutorials.Views;
using Zenject;

namespace _Source.Features.Tutorials.Installation
{
    public class TutorialsInitializer : IInitializable
    {
        [Inject] private TutorialsPrefabConfig _tutorialsPrefabConfig;
        [Inject] private TutorialView.Factory _tutorialViewFactory;

        public void Initialize()
        {
            _tutorialViewFactory.Create(_tutorialsPrefabConfig.ControlsTutorialViewPrefab)
                .Initialize();

            _tutorialViewFactory.Create(_tutorialsPrefabConfig.LifeTutorialViewPrefab)
                .Initialize();

            _tutorialViewFactory.Create(_tutorialsPrefabConfig.NovatarsTutorialViewPrefab)
                .Initialize();
        }
    }
}
