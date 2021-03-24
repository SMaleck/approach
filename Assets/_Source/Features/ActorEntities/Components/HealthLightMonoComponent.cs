using _Source.Features.ActorEntities.Config;
using _Source.Features.Actors.DataComponents;
using UniRx;
using UnityEngine;

namespace _Source.Features.ActorEntities.Components
{
    public class HealthLightMonoComponent : AbstractMonoComponent
    {
        [SerializeField] private Light _headLight;
        [SerializeField] private LightIntensityConfig _intensityConfig;

        private HealthDataComponent _healthDataComponent;

        protected override void OnSetup()
        {
            _healthDataComponent = Actor.Get<HealthDataComponent>();
        }

        protected override void OnStart()
        {
            _healthDataComponent.RelativeHealth
                .Subscribe(_ => UpdateHealthBasedLight())
                .AddTo(Disposer);
        }

        private void UpdateHealthBasedLight()
        {
            var intensity = _intensityConfig.DefaultIntensity * _healthDataComponent.RelativeHealth.Value;
            SetLight(_headLight.color, (float)intensity);
        }

        private void SetLight(Color color, float intensity)
        {
            _headLight.color = color;
            _headLight.intensity = intensity;
        }
    }
}
