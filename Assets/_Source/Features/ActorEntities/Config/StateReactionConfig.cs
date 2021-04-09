using _Source.App;
using UnityEngine;

namespace _Source.Features.ActorEntities.Config
{
    [CreateAssetMenu(fileName = nameof(StateReactionConfig), menuName = Constants.ConfigMenu + nameof(StateReactionConfig))]
    public class StateReactionConfig : ScriptableObject
    {
        [Header("Friend")]
        [SerializeField] private float _friendGrowScale;
        public float FriendGrowScale => _friendGrowScale;

        [SerializeField] private float _friendGrowSeconds;
        public float FriendGrowSeconds => _friendGrowSeconds;

        [Header("Enemy")]
        [SerializeField] private float _enemyShakeStrength;
        public float EnemyShakeStrength => _enemyShakeStrength;

        [SerializeField] private float _enemyShakeSeconds;
        public float EnemyShakeSeconds => _enemyShakeSeconds;

        [SerializeField] private int _enemyShakeVibration;
        public int EnemyShakeVibration => _enemyShakeVibration;
    }
}
