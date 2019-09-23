using _Source.Features.NovatarSpawning;
using _Source.Util;
using DG.Tweening;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Novatar
{
    // ToDo NovatarFacade became a bit of an ugly amalgamation of Features, split this up
    public class NovatarFacade : AbstractDisposable, INovatar, IEntityPoolItem<IMonoEntity>
    {
        public class Factory : PlaceholderFactory<NovatarEntity, NovatarStateModel, NovatarFacade> { }

        private readonly NovatarEntity _novatarEntity;
        private readonly NovatarStateModel _novatarStateModel;
        private readonly NovatarConfig _novatarConfig;

        public IMonoEntity Entity => this;

        public Transform LocomotionTarget => _novatarEntity.LocomotionTarget;
        public Transform RotationTarget => _novatarEntity.RotationTarget;

        public bool IsActive => _novatarEntity.IsActive;
        public Vector3 Position => _novatarEntity.Position;
        public Quaternion Rotation => _novatarEntity.Rotation;
        public Vector3 Size => _novatarEntity.Size;
        public string ToDebugString() => _novatarEntity.ToDebugString();

        public float SqrRange => Mathf.Pow(_novatarConfig.Range, 2);
        public float SqrTargetReachedThreshold => Mathf.Pow(_novatarConfig.TargetReachedThreshold, 2);

        private readonly SerialDisposable _tweenDisposer;
        private readonly Tween _lightsOnTween;

        public NovatarFacade(
            NovatarEntity novatarEntity,
            NovatarStateModel novatarStateModel,
            NovatarConfig novatarConfig)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _novatarConfig = novatarConfig;

            _novatarStateModel.IsAlive
                .Subscribe(isAlive =>
                {
                    _novatarEntity.SetActive(isAlive);
                    IsFree = !isAlive;
                })
                .AddTo(Disposer);

            _novatarStateModel.SpawnPosition
                .Subscribe(_novatarEntity.SetPosition)
                .AddTo(Disposer);

            _novatarStateModel.CurrentEntityState
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
            _novatarStateModel.SetCurrentEntityState(entityState);
        }

        public void SetCurrentDistanceToAvatar(float value)
        {
            _novatarStateModel.SetCurrentDistanceToAvatar(value);
        }

        public void Deactivate()
        {
            _novatarStateModel.SetIsAlive(false);
        }

        public void Reset(Vector3 spawnPosition)
        {
            _novatarStateModel.SetCurrentEntityState(EntityState.Spawning);
            _novatarStateModel.SetSpawnPosition(spawnPosition);

            UpdateLightColor(true);

            _novatarStateModel.PublishOnReset();

            _novatarStateModel.SetIsAlive(true);
        }

        public float GetSquaredDistanceTo(IMonoEntity otherEntity)
        {
            return _novatarEntity.GetSquaredDistanceTo(otherEntity);
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
            var relationship = _novatarStateModel.CurrentEntityState.Value;
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
