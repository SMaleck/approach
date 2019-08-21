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
        private class RelationshipVisualsConfig
        {
            [SerializeField] private RelationshipStatus _relationship;
            public RelationshipStatus Relationship => _relationship;

            [SerializeField] private Color _lightColor;
            public Color LightColor => _lightColor;
        }

        [SerializeField] private NovatarEntity _novatarPrefab;
        public NovatarEntity NovatarPrefab => _novatarPrefab;


        [Header("Movement")]
        [Range(0.001f, 5)]
        [SerializeField] private float _movementTargetAccuracy;
        public float MovementTargetAccuracy => _movementTargetAccuracy;

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

        [Header("Damage")]
        [SerializeField] private double _touchDamage;
        public double TouchDamage => _touchDamage;

        [Header("Visuals")]
        [SerializeField] private List<RelationshipVisualsConfig> _relationshipVisualsConfigs;

        [SerializeField] private float _lightColorFadeSeconds;
        public float LightColorFadeSeconds => _lightColorFadeSeconds;

        [SerializeField] private float _lightDefaultIntensity;
        public float LightDefaultIntensity => _lightDefaultIntensity;

        [SerializeField] private float _lightFlashIntensity;
        public float LightFlashIntensity => _lightFlashIntensity;

        public Color GetLightColor(RelationshipStatus relationship)
        {
            return _relationshipVisualsConfigs
                .First(config => config.Relationship == relationship)
                .LightColor;
        }
    }
}
