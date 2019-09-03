using _Source.App;
using UnityEngine;

namespace _Source.Entities.Avatar
{
    [CreateAssetMenu(fileName = nameof(AvatarConfig), menuName = Constants.ConfigRootPath + "/" + nameof(AvatarConfig))]
    public class AvatarConfig : ScriptableObject
    {
        [SerializeField] private AvatarEntity _avatarPrefab;
        public AvatarEntity AvatarPrefab => _avatarPrefab;


        [Header("Movement")]
        [SerializeField] private float _speed;
        public float Speed => _speed;

        [Range(0.1f, 60)]
        [SerializeField] private float _turnSpeed;
        public float TurnSpeed => _turnSpeed;

        [SerializeField] private float _turnAngleThreshold;
        public float TurnAngleThreshold => _turnAngleThreshold;

        [Header("Health")]
        [SerializeField] private double _health;
        public double Health => _health;
    }
}
