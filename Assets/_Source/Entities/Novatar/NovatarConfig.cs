using _Source.App;
using UnityEngine;

namespace _Source.Entities.Novatar
{
    [CreateAssetMenu(fileName = nameof(NovatarConfig), menuName = Constants.ConfigRootPath + "/" + nameof(NovatarConfig))]
    public class NovatarConfig : ScriptableObject
    {
        [SerializeField] private NovatarEntity _novatarPrefab;
        public NovatarEntity NovatarPrefab => _novatarPrefab;


        [Header("Movement")]
        [SerializeField] private float _range;
        public float Range => _range;

        [Range(0.001f, 5)]
        [SerializeField] private float _targetReachedThreshold;
        public float TargetReachedThreshold => _targetReachedThreshold;

        [Range(0.1f, 20)]
        [SerializeField] private float _movementSpeed;
        public float MovementSpeed => _movementSpeed;

        [Range(0.1f, 60)]
        [SerializeField] private float _turnSpeed;
        public float TurnSpeed => _turnSpeed;

        [SerializeField] private float _turnAngleThreshold;
        public float TurnAngleThreshold => _turnAngleThreshold;
    }
}
