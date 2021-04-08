using _Source.App;
using _Source.Features.Tutorials.Views;
using UnityEngine;

namespace _Source.Features.Tutorials.Config
{
    [CreateAssetMenu(fileName = nameof(TutorialsPrefabConfig), menuName = Constants.ConfigMenu + nameof(TutorialsPrefabConfig))]
    public class TutorialsPrefabConfig : ScriptableObject
    {
        [SerializeField] private TutorialView _controlsTutorialViewPrefab;
        public TutorialView ControlsTutorialViewPrefab => _controlsTutorialViewPrefab;

        [SerializeField] private TutorialView _lifeTutorialViewPrefab;
        public TutorialView LifeTutorialViewPrefab => _lifeTutorialViewPrefab;

        [SerializeField] private TutorialView _novatarsTutorialViewPrefab;
        public TutorialView NovatarsTutorialViewPrefab => _novatarsTutorialViewPrefab;
    }
}
