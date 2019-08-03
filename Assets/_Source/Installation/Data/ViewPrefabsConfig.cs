using _Source.App;
using _Source.Features.GameRound;
using _Source.Features.SurvivalStats;
using UnityEngine;

namespace _Source.Installation.Data
{
    [CreateAssetMenu(fileName = nameof(ViewPrefabsConfig), menuName = Constants.ConfigRootPath + "/" + nameof(ViewPrefabsConfig))]
    public class ViewPrefabsConfig : ScriptableObject
    {
        [SerializeField] private SurvivalStatsView _survivalStatsViewPrefab;
        public SurvivalStatsView SurvivalStatsViewPrefab => _survivalStatsViewPrefab;

        [SerializeField] private RoundEndedView _roundEndedViewPrefab;
        public RoundEndedView RoundEndedViewPrefab => _roundEndedViewPrefab;
    }
}
