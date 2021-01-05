using _Source.App;
using _Source.Features.Movement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using _Source.Features.ActorBehaviours.Sensors.Data;
using UnityEngine;

namespace _Source.Entities.Novatar
{
    [CreateAssetMenu(fileName = nameof(NovatarConfig), menuName = Constants.ConfigRootPath + "/" + nameof(NovatarConfig))]
    public class NovatarConfig : ScriptableObject
    {
        [Serializable]
        private class RelationshipVisualsConfig
        {
            [SerializeField] private EntityState _relationship;
            public EntityState Relationship => _relationship;

            [SerializeField] private Color _lightColor;
            public Color LightColor => _lightColor;
        }

        [SerializeField] private NovatarEntity _novatarPrefab;
        public NovatarEntity NovatarPrefab => _novatarPrefab;

        [Header("Movement")]
        [SerializeField] private MovementConfig _movementConfig;
        public MovementConfig MovementConfig => _movementConfig;

        [Header("Sensory System")]
        [SerializeField] private RangeSensorConfig _rangeSensorConfig;
        public RangeSensorConfig RangeSensorConfig => _rangeSensorConfig;

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

        public Color GetLightColor(EntityState relationship)
        {
            return _relationshipVisualsConfigs
                .First(config => config.Relationship == relationship)
                .LightColor;
        }
    }
}
