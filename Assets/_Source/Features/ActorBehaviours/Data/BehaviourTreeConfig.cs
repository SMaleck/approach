using System;
using System.Collections.Generic;
using _Source.App;
using _Source.Entities.Novatar;
using UnityEngine;

namespace _Source.Features.ActorBehaviours.Data
{
    [CreateAssetMenu(fileName = nameof(BehaviourTreeConfig), menuName = Constants.ConfigRootPath + "/" + nameof(BehaviourTreeConfig))]
    public class BehaviourTreeConfig : ScriptableObject
    {
        [Serializable]
        public class UnacquaintedRelationshipConfig
        {
            [Range(0, 600)]
            [SerializeField] private double _evaluationTimeoutSeconds;
            public double EvaluationTimeoutSeconds => _evaluationTimeoutSeconds;

            [Range(0, 1)]
            [SerializeField] private float _timeBasedSwitchChance;
            public float TimeBasedSwitchChance => _timeBasedSwitchChance;

            [SerializeField] private List<RelationshipSwitchWeightItem> _relationshipSwitchWeights;
            public IReadOnlyList<RelationshipSwitchWeightItem> RelationshipSwitchWeights => _relationshipSwitchWeights;
        }

        [Serializable]
        public class RelationshipSwitchWeightItem
        {
            [SerializeField] private EntityState _switchToRelationship;
            public EntityState SwitchToRelationship => _switchToRelationship;

            [Range(0, 1)]
            [SerializeField] private float _weightedChance;
            public float WeightedChance => _weightedChance;
        }

        [Header("Unacquainted Config")]
        [SerializeField] private UnacquaintedRelationshipConfig _unacquaintedConfig;
        public UnacquaintedRelationshipConfig UnacquaintedConfig => _unacquaintedConfig;

        [Header("Friend Behaviour Settings")]
        [Range(0, 600)]
        [SerializeField] private double _maxSecondsToFallBehind;
        public double MaxSecondsToFallBehind => _maxSecondsToFallBehind;

        [Header("Enemy Behaviour Settings")]
        [Range(0, 600)]
        [SerializeField] private double _enemyLeavingTimeoutSeconds;
        public double EnemyLeavingTimeoutSeconds => _enemyLeavingTimeoutSeconds;
    }
}
