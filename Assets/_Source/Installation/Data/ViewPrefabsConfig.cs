using _Source.App;
using _Source.Features.AvatarState;
using _Source.Features.GameRound;
using _Source.Features.SceneManagement;
using _Source.Features.TitleMenu;
using _Source.Features.UserInput;
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

        [SerializeField] private SettingsView _settingsViewPrefab;
        public SettingsView SettingsViewPrefab => _settingsViewPrefab;

        [SerializeField] private HowToPlayView _howToPlayViewPrefab;
        public HowToPlayView HowToPlayViewPrefab => _howToPlayViewPrefab;

        [Header("Game Views")]
        [SerializeField] private SurvivalStatsView _survivalStatsViewPrefab;
        public SurvivalStatsView SurvivalStatsViewPrefab => _survivalStatsViewPrefab;

        [SerializeField] private RoundEndedView _roundEndedViewPrefab;
        public RoundEndedView RoundEndedViewPrefab => _roundEndedViewPrefab;

        [SerializeField] private VirtualJoystickView _virtualJoystickViewPrefab;
        public VirtualJoystickView VirtualJoystickViewPrefab => _virtualJoystickViewPrefab;
    }
}
