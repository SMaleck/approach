using _Source.App;
using UnityEngine;

namespace _Source.Entities
{
    [CreateAssetMenu(fileName = nameof(AvatarConfig), menuName = Constants.ConfigRootPath + "/" + nameof(AvatarConfig))]
    public class AvatarConfig : ScriptableObject
    {
        [SerializeField] private Avatar _avatarPrefab;
        public Avatar AvatarPrefab => _avatarPrefab;

        [SerializeField] private float _speed;
        public float Speed => _speed;
    }
}
