using _Source.App;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Source.Features.NovatarBehaviour.Data
{
    [CreateAssetMenu(fileName = nameof(BehaviourTreeConfig), menuName = Constants.ConfigRootPath + "/" + nameof(BehaviourTreeConfig))]
    public class BehaviourTreeConfig : ScriptableObject
    {
        [Serializable]
        private class RelationshipConfigItem
        {
            [SerializeField] private RelationshipStatus _relationship;
            public RelationshipStatus Relationship => _relationship;

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
            [SerializeField] private RelationshipStatus _switchToRelationship;
            public RelationshipStatus SwitchToRelationship => _switchToRelationship;

            [Range(0, 1)]
            [SerializeField] private float _weightedChance;
            public float WeightedChance => _weightedChance;
        }

        [Header("Relationship Config Items")]
        [SerializeField] private List<RelationshipConfigItem> _relationshipConfigItems;

        [Header("Friend Tree Settings")]
        [Range(0, 600)]
        [SerializeField] private double _maxSecondsToFallBehind;
        public double MaxSecondsToFallBehind => _maxSecondsToFallBehind;

        private RelationshipConfigItem GetRelationshipConfigItem(RelationshipStatus relationshipStatus)
        {
            return _relationshipConfigItems
                .First(item => item.Relationship == relationshipStatus);
        }

        public double GetEvaluationTimeoutSeconds(RelationshipStatus relationshipStatus)
        {
            return GetRelationshipConfigItem(relationshipStatus)
                .EvaluationTimeoutSeconds;
        }

        public float GetTimeBasedSwitchChance(RelationshipStatus relationshipStatus)
        {
            return GetRelationshipConfigItem(relationshipStatus)
                .TimeBasedSwitchChance;
        }

        public IReadOnlyList<RelationshipSwitchWeightItem> GetRelationshipSwitchWeights(RelationshipStatus relationshipStatus)
        {
            return GetRelationshipConfigItem(relationshipStatus)
                .RelationshipSwitchWeights;
        }
    }
}
