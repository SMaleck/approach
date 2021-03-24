using _Source.App;
using _Source.Entities.Novatar;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Source.Features.ActorEntities.Config
{
    [CreateAssetMenu(fileName = nameof(LightRelationshipConfig), menuName = Constants.ConfigMenu + nameof(LightRelationshipConfig))]
    public class LightRelationshipConfig : ScriptableObject
    {
        [Serializable]
        private class RelationshipVisualsConfig
        {
            [SerializeField] private EntityState _relationship;
            public EntityState Relationship => _relationship;

            [SerializeField] private Color _lightColor;
            public Color LightColor => _lightColor;
        }

        [SerializeField] private float _colorFadeSeconds;
        public float ColorFadeSeconds => _colorFadeSeconds;

        [SerializeField] private List<RelationshipVisualsConfig> _relationshipVisualsConfigs;
        private Dictionary<EntityState, RelationshipVisualsConfig> _cache;

        public Color GetLightColor(EntityState relationship)
        {
            return GetCache()[relationship]
                .LightColor;
        }

        private Dictionary<EntityState, RelationshipVisualsConfig> GetCache()
        {
            return _cache ?? (_cache = _relationshipVisualsConfigs.ToDictionary(e => e.Relationship));
        }
    }
}
