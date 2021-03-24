using _Source.App;
using UnityEngine;

namespace _Source.Features.ActorEntities.Avatar.Config
{
    [CreateAssetMenu(fileName = nameof(AvatarConfig), menuName = Constants.ConfigMenu + nameof(AvatarConfig))]
    public class AvatarConfig : ScriptableObject
    {
        [SerializeField] private MonoEntity _avatarPrefab;
        public MonoEntity AvatarPrefab => _avatarPrefab;
    }
}
