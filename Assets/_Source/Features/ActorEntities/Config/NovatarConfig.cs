using _Source.App;
using UnityEngine;

namespace _Source.Features.ActorEntities.Novatar.Config
{
    [CreateAssetMenu(fileName = nameof(NovatarConfig), menuName = Constants.ConfigMenu + nameof(NovatarConfig))]
    public class NovatarConfig : ScriptableObject
    {
        [SerializeField] private MonoEntity _novatarPrefab;
        public MonoEntity NovatarPrefab => _novatarPrefab;
    }
}
