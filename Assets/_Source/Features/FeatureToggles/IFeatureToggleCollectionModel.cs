namespace _Source.Features.FeatureToggles
{
    public interface IFeatureToggleCollectionModel
    {
        ITogglableFeature this[FeatureId id] { get; }
    }
}