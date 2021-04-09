using _Source.Entities.Novatar;
using _Source.Features.ActorEntities.Config;
using _Source.Features.Actors.DataComponents;
using DG.Tweening;
using Packages.RxUtils;
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
        private HealthDataComponent _healthDataComponent;

        private SerialDisposable _tweenDisposer;
        private bool _isLightOn = false;

        protected override void OnSetup()
        {
            _relationshipDataComponent = Actor.Get<RelationshipDataComponent>();
            _healthDataComponent = Actor.Get<HealthDataComponent>();
        }

        protected override void OnStart()
        {
            ResetLight();

            _tweenDisposer = new SerialDisposable().AddTo(Disposer);

            _relationshipDataComponent.Relationship
                .Pairwise()
                .Subscribe(OnRelationshipSwitched)
                .AddTo(Disposer);

            _healthDataComponent.RelativeHealth
                .Where(_ => _isLightOn)
                .Subscribe(_ => UpdateHealthBasedLight())
                .AddTo(Disposer);

            Observable.NextFrame()
                .Subscribe(_ => TurnOnLights())
                .AddTo(Disposer);
        }

        protected override void OnStop()
        {
            _tweenDisposer?.Dispose();
            ResetLight();
        }

        private void ResetLight()
        {
            _isLightOn = false;

            var lightColor = _relationshipConfig.GetLightColor(EntityState.Spawning);
            SetLight(lightColor, 0);
        }

        private void TurnOnLights()
        {
            _headLight.intensity = 0;

            DOTween.Sequence()
                .Append(_headLight
                    .DOIntensity(_intensityConfig.FlashIntensity, _relationshipConfig.ColorFadeSeconds)
                    .SetEase(Ease.InOutCubic))
                .Append(_headLight
                    .DOIntensity(_intensityConfig.DefaultIntensity, _relationshipConfig.ColorFadeSeconds)
                    .SetEase(Ease.InOutCubic))
                .AppendCallback(() =>
                {
                    _isLightOn = true;
                    UpdateHealthBasedLight();
                })
                .AddTo(Disposer);
        }

        private void OnRelationshipSwitched(Pair<EntityState> relationshipPair)
        {
            if (!IsValidPair(relationshipPair))
            {
                return;
            }

            ToStateColor(relationshipPair.Current);
        }

        private bool IsValidPair(Pair<EntityState> pair)
        {
            if (pair.Previous == EntityState.Spawning ||
                pair.Current == EntityState.Spawning)
            {
                return false;
            }
            if (pair.Previous == EntityState.Unacquainted &&
                pair.Current == EntityState.Neutral)
            {
                return false;
            }

            return true;
        }

        private void ToStateColor(EntityState state)
        {
            var lightColor = _relationshipConfig.GetLightColor(state);
            var lightSequenceDisposer = new CompositeDisposable();

            DOTween.Sequence()
                .Join(CreateLightIntensityTween())
                .Join(CreateLightColorTween(lightColor))
                .OnComplete(UpdateHealthBasedLight)
                .AddTo(lightSequenceDisposer);

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
