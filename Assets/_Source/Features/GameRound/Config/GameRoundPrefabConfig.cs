using _Source.App;
using _Source.Features.GameRound.Duration;
using UnityEngine;

namespace _Source.Features.GameRound.Config
{
    [CreateAssetMenu(fileName = nameof(GameRoundPrefabConfig), menuName = Constants.ConfigMenu + nameof(GameRoundPrefabConfig))]
    public class GameRoundPrefabConfig : ScriptableObject
    {
        [SerializeField] private GameRoundDurationView _gameRoundDurationViewPrefab;
        public GameRoundDurationView GameRoundDurationViewPrefab => _gameRoundDurationViewPrefab;
    }
}
