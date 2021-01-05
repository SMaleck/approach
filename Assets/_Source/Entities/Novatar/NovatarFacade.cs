using _Source.Features.ActorBehaviours.NovatarSpawning;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Novatar
{
    // ToDo NovatarFacade became a bit of an ugly amalgamation of Features, split this up
    public class NovatarFacade : AbstractDisposable, INovatar, IEntityPoolItem<IMonoEntity>
    {
        public class Factory : PlaceholderFactory<NovatarEntity, IActorStateModel, NovatarFacade> { }

        private readonly NovatarEntity _novatarEntity;
        private readonly IActorStateModel _actorStateModel;
        private readonly NovatarConfig _novatarConfig;

        public IMonoEntity Entity => this;

        public Transform LocomotionTarget => _novatarEntity.LocomotionTarget;
        public Transform RotationTarget => _novatarEntity.RotationTarget;

        public bool IsActive => _novatarEntity.IsActive;
        public Vector3 Position => _novatarEntity.Position;
        public Quaternion Rotation => _novatarEntity.Rotation;
        public Vector3 Size => _novatarEntity.Size;
        public string ToDebugString() => _novatarEntity.ToDebugString();

        private readonly SerialDisposable _tweenDisposer;
        private readonly Tween _lightsOnTween;

        private readonly HealthDataComponent _healthDataComponent;
        private readonly OriginDataComponent _originDataComponent;
        private readonly RelationshipDataComponent _relationshipDataComponent;

        public NovatarFacade(
            NovatarEntity novatarEntity,
            IActorStateModel actorStateModel,
            NovatarConfig novatarConfig)
        {
            _novatarEntity = novatarEntity;
            _actorStateModel = actorStateModel;
            _novatarConfig = novatarConfig;

            _healthDataComponent = _actorStateModel.Get<HealthDataComponent>();
            _originDataComponent = _actorStateModel.Get<OriginDataComponent>();
            _relationshipDataComponent = _actorStateModel.Get<RelationshipDataComponent>();

            _healthDataComponent.IsAlive
                .Subscribe(isAlive =>
                {
                    _novatarEntity.SetActive(isAlive);
                    IsFree = !isAlive;
                })
                .AddTo(Disposer);

            _originDataComponent.SpawnPosition
                .Subscribe(_novatarEntity.SetPosition)
                .AddTo(Disposer);

            _relationshipDataComponent.Relationship
                .Where(_ => IsActive)
                .Pairwise()
                .Subscribe(OnRelationshipSwitched)
                .AddTo(Disposer);

            _tweenDisposer = new SerialDisposable().AddTo(Disposer);

            _lightsOnTween = CreateLightsOnTween();
        }

        public bool IsFree { get; private set; }

        public void SwitchToEntityState(EntityState entityState)
        {
            _relationshipDataComponent.SetRelationship(entityState);
        }

        public void Deactivate()
        {
            _healthDataComponent.SetIsAlive(false);
        }

        public void Reset(Vector3 spawnPosition)
        {
            _relationshipDataComponent.SetRelationship(EntityState.Spawning);
            _originDataComponent.SetSpawnPosition(spawnPosition);

            UpdateLightColor(true);

            _actorStateModel.Reset();
        }

        public void ResetIdleTimeouts()
        {
            _actorStateModel.PublishOnResetIdleTimeouts();
        }

        public void TurnLightsOn()
        {
            _lightsOnTween.Restart();
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
                .Join(CreateLightColorTween(lightColor))
                .AddTo(lightSequenceDisposer, TweenDisposalBehaviour.Rewind);

            _tweenDisposer.Disposable = lightSequenceDisposer;
        }

        private Tween CreateLightIntensityTween()
        {
            _novatarEntity.HeadLight.intensity = _novatarConfig.LightDefaultIntensity;

            return _novatarEntity.HeadLight
                .DOIntensity(_novatarConfig.LightFlashIntensity, _novatarConfig.LightColorFadeSeconds / 2)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);
        }

        private Tween CreateLightColorTween(Color targetColor)
        {
            return _novatarEntity.HeadLight
                .DOColor(targetColor, _novatarConfig.LightColorFadeSeconds)
                .SetEase(Ease.InOutCubic);
        }

        public Tween CreateLightsOnTween()
        {
            _novatarEntity.HeadLight.intensity = 0;

            var tween = _novatarEntity.HeadLight
                .DOIntensity(_novatarConfig.LightDefaultIntensity, _novatarConfig.LightColorFadeSeconds)
                .SetEase(Ease.InOutCubic)
                .SetAutoKill(false)
                .Pause()
                .AddTo(Disposer, TweenDisposalBehaviour.Rewind);
            tween.ForceInit();

            _novatarEntity.HeadLight.intensity = 1;

            return tween;
        }

        private void SetLight(Color color, float intensity)
        {
            _novatarEntity.HeadLight.color = color;
            _novatarEntity.HeadLight.intensity = intensity;
        }
    }
}
