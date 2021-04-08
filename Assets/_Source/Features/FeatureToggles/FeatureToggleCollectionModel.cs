using System.Collections.Generic;
using System.Linq;

namespace _Source.Features.FeatureToggles
{
    public class FeatureToggleCollectionModel : IFeatureToggleCollectionModel
    {
        private IReadOnlyDictionary<FeatureId, ITogglableFeature> _features;

        public ITogglableFeature this[FeatureId id] => _features[id];

        public FeatureToggleCollectionModel(List<ITogglableFeature> features)
        {
            _features = features.ToDictionary(e => e.FeatureId);
        }
    }
}
