using _Source.App;
using UnityEngine;

namespace _Source.Features.ActorEntities.Config
{
    [CreateAssetMenu(fileName = nameof(NovatarSpawnerConfig), menuName = Constants.ConfigMenu + nameof(NovatarSpawnerConfig))]
    public class NovatarSpawnerConfig : ScriptableObject
    {
        [Range(0.1f, 20)]
        [SerializeField] private float _spawnIntervalSeconds;
        public float SpawnIntervalSeconds => _spawnIntervalSeconds;

        [Range(0, 100)]
        [SerializeField] private int _maxActiveSpawns;
        public float MaxActiveSpawns => _maxActiveSpawns;

        [Range(0, 5)]
        [SerializeField] private float _spawnPositionOffset;
        public float SpawnPositionOffset => _spawnPositionOffset;
    }
}
