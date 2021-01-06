using _Source.App;
using _Source.Entities.Avatar;
using UnityEngine;

namespace _Source.Features.ActorEntities.Avatar.Config
{
    [CreateAssetMenu(fileName = nameof(AvatarConfig), menuName = Constants.ConfigMenu + nameof(AvatarConfig))]
    public class AvatarConfig : ScriptableObject
    {
        [SerializeField] private AvatarEntity _avatarPrefab;
        public AvatarEntity AvatarPrefab => _avatarPrefab;

        [SerializeField] private float _maxLightIntensity;
        public float MaxLightIntensity => _maxLightIntensity;
    }
}
