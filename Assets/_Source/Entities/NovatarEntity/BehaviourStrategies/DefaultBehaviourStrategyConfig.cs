using _Source.App;
using UnityEngine;

namespace _Source.Entities.NovatarEntity.BehaviourStrategies
{
    [CreateAssetMenu(fileName = nameof(DefaultBehaviourStrategyConfig), menuName = Constants.ConfigRootPath + "/" + nameof(DefaultBehaviourStrategyConfig))]
    public class DefaultBehaviourStrategyConfig : ScriptableObject
    {
        [SerializeField] private double _hoverTimeoutSeconds;
        public double HoverTimeoutSeconds => _hoverTimeoutSeconds;

        [SerializeField] private double _incrementalMovementDistanceUnits;
        public double IncrementalMovementDistanceUnits => _incrementalMovementDistanceUnits;

        [SerializeField] private double _targetedMovementChance;
        public double TargetedMovementChance => _targetedMovementChance;
    }
}
