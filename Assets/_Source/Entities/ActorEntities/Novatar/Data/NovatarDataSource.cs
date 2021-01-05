using _Source.App;
using UnityEngine;

namespace _Source.Entities.ActorEntities.Novatar.Data
{
    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(NovatarDataSource), fileName = nameof(NovatarDataSource))]
    public class NovatarDataSource : ScriptableObject
    {
        [SerializeField, Range(1, 100)]
        private int _maxHealth;
        public int MaxHealth => _maxHealth;

        [SerializeField, Range(0.01f, 100f)]
        private double _moveSpeed;
        public double MoveSpeed => _moveSpeed;
    }
}
