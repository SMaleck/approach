using _Source.App;
using UnityEngine;

namespace _Source.Entities.Avatar
{
    [CreateAssetMenu(fileName = nameof(AvatarConfig), menuName = Constants.ConfigRootPath + "/" + nameof(AvatarConfig))]
    public class AvatarConfig : ScriptableObject
    {
        [SerializeField] private AvatarEntity _avatarPrefab;
        public AvatarEntity AvatarPrefab => _avatarPrefab;

        [SerializeField] private float _speed;
        public float Speed => _speed;
    }
}
