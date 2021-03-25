using _Source.Entities.Novatar;
using _Source.Features.ActorEntities.Config;
using _Source.Features.Actors.DataComponents;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace _Source.Features.ActorEntities.Components
{
    public class HeadLightMonoComponent : AbstractMonoComponent
    {
        [SerializeField] private Light _headLight;
        [SerializeField] private LightRelationshipConfig _relationshipConfig;
        [SerializeField] private LightIntensityConfig _intensityConfig;

        private RelationshipDataComponent _relationshipDataComponent;
        private LightDataComponent _lightDataComponent;
        private HealthDataComponent _healthDataComponent;

        private SerialDisposable _tweenDisposer;
        private Tween _lightsOnTween;

        protected override void OnSetup()
        {
            _relationshipDataComponent = Actor.Get<RelationshipDataComponent>();
            _lightDataComponent = Actor.Get<LightDataComponent>();
            _healthDataComponent = Actor.Get<HealthDataComponent>();
        }

        protected override void OnStart()
        {
            _tweenDisposer = new SerialDisposable().AddTo(Disposer);

            _lightsOnTween = CreateLightsOnTween();

            _relationshipDataComponent.Relationship
                .Pairwise()
                .Subscribe(OnRelationshipSwitched)
                .AddTo(Disposer);

            _lightDataComponent.OnLightsSwitchedOn
                .Subscribe(_ => _lightsOnTween.Restart())
                .AddTo(Disposer);

            _healthDataComponent.RelativeHealth
                .Subscribe(_ => UpdateHealthBasedLight())
                .AddTo(Disposer);

            UpdateLightColor(true);
        }

        protected override void OnStop()
        {
            _tweenDisposer?.Dispose();
        }

        private void OnRelationshipSwitched(Pair<EntityState> relationshipPair)
        {
            var previousIsUnacquainted = relationshipPair.Previous == EntityState.Unacquainted;
            var currentIsNeutral = relationshipPair.Current == EntityState.Neutral;

            if (previousIsUnacquainted && currentIsNeutral)
            {
                return;
            }

            var isSilentVisualUpdate = relationshipPair.Current == EntityState.Unacquainted;
            UpdateLightColor(isSilentVisualUpdate);
        }

        private void UpdateLightColor(bool forceInstant = false)
        {
            var relationship = _relationshipDataComponent.Relationship.Value;
            var lightColor = _relationshipConfig.GetLightColor(relationship);

            if (forceInstant)
            {
                _tweenDisposer.Disposable?.Dispose();
                SetLight(lightColor, _intensityConfig.DefaultIntensity);
                return;
            }

            var lightSequenceDisposer = new CompositeDisposable();

            DOTween.Sequence()
                .Join(CreateLightIntensityTween())
                .Join(CreateLightColorTween(lightColor))
                .OnComplete(UpdateHealthBasedLight);

            _tweenDisposer.Disposable = lightSequenceDisposer;
        }

        private Tween CreateLightIntensityTween()
        {
            _headLight.intensity = _intensityConfig.DefaultIntensity;

            return _headLight
                .DOIntensity(_intensityConfig.FlashIntensity, _relationshipConfig.ColorFadeSeconds / 2)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);
        }

        private Tween CreateLightColorTween(Color targetColor)
        {
            return _headLight
                .DOColor(targetColor, _relationshipConfig.ColorFadeSeconds)
                .SetEase(Ease.InOutCubic);
        }

        private Tween CreateLightsOnTween()
        {
            _headLight.intensity = 0;

            var tween = _headLight
                .DOIntensity(_intensityConfig.DefaultIntensity, _relationshipConfig.ColorFadeSeconds)
                .SetEase(Ease.InOutCubic)
                .SetAutoKill(false)
                .Pause();
            tween.ForceInit();

            _headLight.intensity = 1;

            return tween;
        }

        private void SetLight(Color color, float intensity)
        {
            _headLight.color = color;
            _headLight.intensity = intensity;
        }

        private void UpdateHealthBasedLight()
        {
            var intensity = _intensityConfig.DefaultIntensity * _healthDataComponent.RelativeHealth.Value;
            SetLight(_headLight.color, (float)intensity);
        }
    }
}
