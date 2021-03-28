using _Source.App;
using UnityEngine;

namespace _Source.Features.ActorEntities.Config
{
    [CreateAssetMenu(fileName = nameof(ActorEntitiesConfig), menuName = Constants.ConfigMenu + nameof(ActorEntitiesConfig))]
    public class ActorEntitiesConfig : ScriptableObject
    {
        [SerializeField] private MonoEntity _avatarPrefab;
        public MonoEntity AvatarPrefab => _avatarPrefab;

        [SerializeField] private MonoEntity _novatarPrefab;
        public MonoEntity NovatarPrefab => _novatarPrefab;
    }
}
