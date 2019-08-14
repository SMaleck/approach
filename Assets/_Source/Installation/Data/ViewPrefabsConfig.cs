using _Source.App;
using _Source.Features.AvatarState;
using _Source.Features.GameRound;
using _Source.Features.SceneManagement;
using _Source.Features.TitleMenu;
using UnityEngine;

namespace _Source.Installation.Data
{
    [CreateAssetMenu(fileName = nameof(ViewPrefabsConfig), menuName = Constants.ConfigRootPath + "/" + nameof(ViewPrefabsConfig))]
    public class ViewPrefabsConfig : ScriptableObject
    {
        [Header("Global Views")]
        [SerializeField] private LoadingScreenView _loadingScreenViewPrefab;
        public LoadingScreenView LoadingScreenViewPrefab => _loadingScreenViewPrefab;

        [Header("Title Views")]
        [SerializeField] private TitleView _titleViewPrefab;
        public TitleView TitleViewPrefab => _titleViewPrefab;

        [Header("Game Views")]
        [SerializeField] private SurvivalStatsView _survivalStatsViewPrefab;
        public SurvivalStatsView SurvivalStatsViewPrefab => _survivalStatsViewPrefab;

        [SerializeField] private RoundEndedView _roundEndedViewPrefab;
        public RoundEndedView RoundEndedViewPrefab => _roundEndedViewPrefab;
    }
}
