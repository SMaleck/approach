using _Source.App;
using UnityEngine;

namespace _Source.Features.ActorEntities.Avatar.Data
{
    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(AvatarDataSource), fileName = nameof(AvatarDataSource))]
    public class AvatarDataSource : ScriptableObject
    {
        [SerializeField, Range(1, 100)]
        private int _maxHealth;
        public int MaxHealth => _maxHealth;

        [SerializeField, Range(0.01f, 100f)]
        private double _moveSpeed;
        public double MoveSpeed => _moveSpeed;
    }
}
