using UniRx;

namespace _Source.Features.FeatureToggles
{
    public interface ITogglableFeature : IFeature
    {
        IReadOnlyReactiveProperty<bool> IsEnabled { get; }

        void SetIsEnabled(bool isEnabled);
    }
}
