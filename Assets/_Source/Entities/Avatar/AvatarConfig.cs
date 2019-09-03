using _Source.App;
using _Source.Features.Movement.Data;
using UnityEngine;

namespace _Source.Entities.Avatar
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
    }
}
