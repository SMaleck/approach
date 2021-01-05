using _Source.App;
using _Source.Entities.Avatar;
using _Source.Features.Movement.Data;
using UnityEngine;

namespace _Source.Features.ActorEntities.Avatar.Config
{
    [CreateAssetMenu(fileName = nameof(AvatarConfig), menuName = Constants.ConfigRootPath + "/" + nameof(AvatarConfig))]
    public class AvatarConfig : ScriptableObject
    {
        [SerializeField] private AvatarEntity _avatarPrefab;
        public AvatarEntity AvatarPrefab => _avatarPrefab;
        
        [Header("Movement")]
        [SerializeField] private MovementConfig _movementConfig;
        public MovementConfig MovementConfig => _movementConfig;

        [Header("Health")]
        [SerializeField] private double _health;
        public double Health => _health;

        [SerializeField] private float _maxLightIntensity;
        public float MaxLightIntensity => _maxLightIntensity;
    }
}
