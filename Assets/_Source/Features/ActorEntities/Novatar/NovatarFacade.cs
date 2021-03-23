using _Source.Entities.Novatar;
using _Source.Features.ActorEntities.Novatar.Config;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using _Source.Util;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    // ToDo V0 Get IMovableEntity to not be implemented on this
    public class NovatarFacade : AbstractDisposable, IEntityPoolItem<IMonoEntity>, IMovableEntity
    {
        public class Factory : PlaceholderFactory<MonoEntity, IActorStateModel, NovatarFacade> { }

        private readonly MonoEntity _entity;
        private readonly NovatarConfig _novatarConfig;

        private readonly IActorStateModel _actorStateModel;
        private readonly HealthDataComponent _healthDataComponent;
        private readonly OriginDataComponent _originDataComponent;
        private readonly RelationshipDataComponent _relationshipDataComponent;
        private readonly LightDataComponent _lightDataComponent;

        private readonly SerialDisposable _tweenDisposer;
        private readonly Tween _lightsOnTween;

        public IMonoEntity Entity => _entity;

        public Transform LocomotionTarget => _entity.LocomotionTarget;
        public Transform RotationTarget => _entity.RotationTarget;

        // ToDo V0 Most properties below should probably go into another data component
        public string Name => _entity.Name;
        public bool IsActive => _entity.IsActive;
        public Vector3 Position => _entity.Position;
        public Quaternion Rotation => _entity.Rotation;

        public bool IsFree { get; private set; }

        public NovatarFacade(
            MonoEntity entity,
            IActorStateModel actorStateModel,
            NovatarConfig novatarConfig)
        {
            _entity = entity;
            _actorStateModel = actorStateModel;
            _novatarConfig = novatarConfig;

            _entity.Setup(_actorStateModel);

            _healthDataComponent = _actorStateModel.Get<HealthDataComponent>();
            _originDataComponent = _actorStateModel.Get<OriginDataComponent>();
            _relationshipDataComponent = _actorStateModel.Get<RelationshipDataComponent>();
            _lightDataComponent = _actorStateModel.Get<LightDataComponent>();

            _actorStateModel.Get<TransformDataComponent>()
                .SetMonoEntity(_entity);

            _healthDataComponent.IsAlive
                .Subscribe(OnIsAliveChanged)
                .AddTo(Disposer);

            _originDataComponent.SpawnPosition
                .Subscribe(_entity.SetPosition)
                .AddTo(Disposer);

            _relationshipDataComponent.Relationship
                .Where(_ => IsActive)
                .Pairwise()
                .Subscribe(OnRelationshipSwitched)
                .AddTo(Disposer);

            _lightDataComponent.OnLightsSwitchedOn
                .Subscribe(_ => _lightsOnTween.Restart())
                .AddTo(Disposer);

            _tweenDisposer = new SerialDisposable().AddTo(Disposer);
            _lightsOnTween = CreateLightsOnTween();
        }

        public void Reset(Vector3 spawnPosition)
        {
            _relationshipDataComponent.SetRelationship(EntityState.Spawning);
            _originDataComponent.SetSpawnPosition(spawnPosition);

            UpdateLightColor(true);

            _actorStateModel.Reset();
        }

        private void OnIsAliveChanged(bool isAlive)
        {
            _entity.SetActive(isAlive);
            IsFree = !isAlive;

            if (isAlive)
            {
                _entity.StartEntity(new CompositeDisposable());
            }
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
            _entity.HeadLight.intensity = _novatarConfig.LightDefaultIntensity;

            return _entity.HeadLight
                .DOIntensity(_novatarConfig.LightFlashIntensity, _novatarConfig.LightColorFadeSeconds / 2)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);
        }

        private Tween CreateLightColorTween(Color targetColor)
        {
            return _entity.HeadLight
                .DOColor(targetColor, _novatarConfig.LightColorFadeSeconds)
                .SetEase(Ease.InOutCubic);
        }

        private Tween CreateLightsOnTween()
        {
            _entity.HeadLight.intensity = 0;

            var tween = _entity.HeadLight
                .DOIntensity(_novatarConfig.LightDefaultIntensity, _novatarConfig.LightColorFadeSeconds)
                .SetEase(Ease.InOutCubic)
                .SetAutoKill(false)
                .Pause()
                .AddTo(Disposer, TweenDisposalBehaviour.Rewind);
            tween.ForceInit();

            _entity.HeadLight.intensity = 1;

            return tween;
        }

        private void SetLight(Color color, float intensity)
        {
            _entity.HeadLight.color = color;
            _entity.HeadLight.intensity = intensity;
        }
    }
}
