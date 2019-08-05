using _Source.App;
using UnityEngine;

namespace _Source.Entities
{
    [CreateAssetMenu(fileName = nameof(NovatarConfig), menuName = Constants.ConfigRootPath + "/" + nameof(NovatarConfig))]
    public class NovatarConfig : ScriptableObject
    {
        [SerializeField] private Novatar _novatarPrefab;
        public Novatar NovatarPrefab => _novatarPrefab;

        [Header("Movement")]
        [SerializeField] private float _range;
        public float Range => _range;

        [SerializeField] private float _targetReachedThreshold;
        public float TargetReachedThreshold => _targetReachedThreshold;

        [SerializeField] private float _speed;
        public float Speed => _speed;

        [Header("Spawning")]
        [SerializeField] private float _spawnIntervalSeconds;
        public float SpawnIntervalSeconds => _spawnIntervalSeconds;
    }
}
