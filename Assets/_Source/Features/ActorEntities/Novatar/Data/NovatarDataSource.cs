using _Source.App;
using _Source.Features.Movement.Data;
using UnityEngine;

namespace _Source.Features.ActorEntities.Novatar.Data
{
    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(NovatarDataSource), fileName = nameof(NovatarDataSource))]
    public class NovatarDataSource : ScriptableObject
    {
        [SerializeField, Range(1, 10)]
        private int _maxHealth;
        public int MaxHealth => _maxHealth;

        [SerializeField, Range(1, 10)] private int _touchDamage;
        public int TouchDamage => _touchDamage;

        [Header("Movement System Data")]
        [SerializeField] private MovementDataSource _movementDataSource;
        public MovementDataSource MovementDataSource => _movementDataSource;
    }
}
