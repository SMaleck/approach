using _Source.App;
using _Source.Entities.Novatar;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Source.Features.ActorEntities.Novatar.Config
{
    [CreateAssetMenu(fileName = nameof(NovatarConfig), menuName = Constants.ConfigMenu + nameof(NovatarConfig))]
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

        [SerializeField] private MonoEntity _novatarPrefab;
        public MonoEntity NovatarPrefab => _novatarPrefab;

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
