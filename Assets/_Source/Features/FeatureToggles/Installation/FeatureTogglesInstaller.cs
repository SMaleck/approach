using Zenject;

namespace _Source.Features.FeatureToggles.Installation
{
    public class FeatureTogglesInstaller : Installer<FeatureTogglesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<FeatureToggleCollectionModel>().AsSingle();
        }
    }
}
