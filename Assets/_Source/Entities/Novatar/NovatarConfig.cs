using _Source.App;
using _Source.Features.NovatarBehaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Source.Entities.Novatar
{
    [CreateAssetMenu(fileName = nameof(NovatarConfig), menuName = Constants.ConfigRootPath + "/" + nameof(NovatarConfig))]
    public class NovatarConfig : ScriptableObject
    {
        [Serializable]
        private class RelationshipTimeoutItem
        {
            [SerializeField] private RelationshipStatus _relationship;
            public RelationshipStatus Relationship => _relationship;

            [Range(0, 600)]
            [SerializeField] private double _evaluationTimeoutSeconds;
            public double EvaluationTimeoutSeconds => _evaluationTimeoutSeconds;

            [Range(0, 1)]
            [SerializeField] private double _switchChance;
            public double SwitchChance => _switchChance;
        }

        [SerializeField] private NovatarEntity _novatarPrefab;
        public NovatarEntity NovatarPrefab => _novatarPrefab;


        [Header("Movement")]
        [SerializeField] private float _range;
        public float Range => _range;

        [Range(0.001f, 5)]
        [SerializeField] private float _targetReachedThreshold;
        public float TargetReachedThreshold => _targetReachedThreshold;

        [Range(0.1f, 20)]
        [SerializeField] private float _movementSpeed;
        public float MovementSpeed => _movementSpeed;

        [Range(0.1f, 60)]
        [SerializeField] private float _turnSpeed;
        public float TurnSpeed => _turnSpeed;

        [SerializeField] private float _turnAngleThreshold;
        public float TurnAngleThreshold => _turnAngleThreshold;


        [Header("Timeouts")]
        [SerializeField] private List<RelationshipTimeoutItem> _relationshipTimeouts;

        public double GetRelationshipTimeout(RelationshipStatus relationshipStatus)
        {
            return _relationshipTimeouts
                .First(item => item.Relationship == relationshipStatus)
                .EvaluationTimeoutSeconds;
        }

        public double GetRelationshipSwitchChance(RelationshipStatus relationshipStatus)
        {
            return _relationshipTimeouts
                .First(item => item.Relationship == relationshipStatus)
                .SwitchChance;
        }
    }
}
