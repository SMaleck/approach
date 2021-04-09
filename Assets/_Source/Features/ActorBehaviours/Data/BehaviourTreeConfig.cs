using _Source.App;
using _Source.Entities.Novatar;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Source.Features.ActorBehaviours.Data
{
    // ToDo V2 Refactor to use data pattern
    [CreateAssetMenu(fileName = nameof(BehaviourTreeConfig), menuName = Constants.ConfigMenu + nameof(BehaviourTreeConfig))]
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
    }
}
