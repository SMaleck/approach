using _Source.Entities.Novatar;
using _Source.Features.ActorEntities.Novatar.Config;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Components
{
    public class HeadLightMonoComponent : AbstractMonoComponent
    {
        [SerializeField] private Light _headLight;

        // ToDo V1 Replace with generic LightConfig
        private NovatarConfig _novatarConfig;

        private RelationshipDataComponent _relationshipDataComponent;
        private LightDataComponent _lightDataComponent;

        private SerialDisposable _tweenDisposer;
        private Tween _lightsOnTween;

        [Inject]
        private void Inject(NovatarConfig novatarConfig)
        {
            _novatarConfig = novatarConfig;
        }

        protected override void OnSetup()
        {
            _relationshipDataComponent = Actor.Get<RelationshipDataComponent>();
            _lightDataComponent = Actor.Get<LightDataComponent>();
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
            var lightColor = _novatarConfig.GetLightColor(relationship);

            if (forceInstant)
            {
                _tweenDisposer.Disposable?.Dispose();
                SetLight(lightColor, _novatarConfig.LightDefaultIntensity);
                return;
            }

            var lightSequenceDisposer = new CompositeDisposable();

            DOTween.Sequence()
                .Join(CreateLightIntensityTween())
                .Join(CreateLightColorTween(lightColor));

            _tweenDisposer.Disposable = lightSequenceDisposer;
        }

        private Tween CreateLightIntensityTween()
        {
            _headLight.intensity = _novatarConfig.LightDefaultIntensity;

            return _headLight
                .DOIntensity(_novatarConfig.LightFlashIntensity, _novatarConfig.LightColorFadeSeconds / 2)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);
        }

        private Tween CreateLightColorTween(Color targetColor)
        {
            return _headLight
                .DOColor(targetColor, _novatarConfig.LightColorFadeSeconds)
                .SetEase(Ease.InOutCubic);
        }

        private Tween CreateLightsOnTween()
        {
            _headLight.intensity = 0;

            var tween = _headLight
                .DOIntensity(_novatarConfig.LightDefaultIntensity, _novatarConfig.LightColorFadeSeconds)
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
    }
}
