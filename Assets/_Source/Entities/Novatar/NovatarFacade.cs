using _Source.Features.NovatarBehaviour;
using _Source.Features.NovatarSpawning;
using _Source.Util;
using DG.Tweening;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Novatar
{
    public class NovatarFacade : AbstractDisposable, INovatar, IEntityPoolItem<NovatarEntity>
    {
        private readonly NovatarEntity _novatarEntity;
        private readonly NovatarStateModel _novatarStateModel;
        private readonly NovatarConfig _novatarConfig;

        public class Factory : PlaceholderFactory<NovatarEntity, NovatarStateModel, NovatarFacade> { }

        public NovatarEntity Entity => _novatarEntity;
        public bool IsActive => Entity.IsActive;
        public Vector3 Position => Entity.Position;
        public Quaternion Rotation => Entity.Rotation;
        public Vector3 Size => Entity.Size;

        public float SqrRange => Mathf.Pow(_novatarConfig.Range, 2);
        public float SqrTargetReachedThreshold => Mathf.Pow(_novatarConfig.TargetReachedThreshold, 2);

        private readonly SerialDisposable _tweenDisposer;

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
                    Entity.SetActive(isAlive);
                    IsFree = !isAlive;
                })
                .AddTo(Disposer);

            _novatarStateModel.SpawnPosition
                .Subscribe(Entity.SetPosition)
                .AddTo(Disposer);

            _novatarStateModel.CurrentRelationshipStatus
                .Pairwise()
                .Subscribe(OnRelationshipSwitched)
                .AddTo(Disposer);

            _tweenDisposer = new SerialDisposable().AddTo(Disposer);
        }

        public bool IsFree { get; private set; }

        public void Reset(Vector3 spawnPosition)
        {
            _novatarStateModel.PublishOnReset();
            _novatarStateModel.SetSpawnPosition(spawnPosition);

            UpdateLightColor(true);
        }

        public void MoveTowards(Vector3 targetPosition)
        {
            FaceTarget(targetPosition);
            MoveForward();
        }

        public bool IsMovementTargetReached(Vector3 targetPosition)
        {
            var sqrDistanceToTarget = _novatarEntity.GetSquaredDistanceTo(targetPosition);
            return sqrDistanceToTarget <= Mathf.Pow(_novatarConfig.MovementTargetAccuracy, 2);
        }

        public float GetSquaredDistanceTo(AbstractMonoEntity otherEntity)
        {
            return Entity.GetSquaredDistanceTo(otherEntity);
        }

        private void FaceTarget(Vector3 targetPosition)
        {
            var headingToTarget = targetPosition - _novatarEntity.Position;

            if (Vector3.Angle(_novatarEntity.Position, headingToTarget) < _novatarConfig.TurnAngleThreshold)
            {
                return;
            }

            var rotation = Quaternion.Slerp(
                _novatarEntity.Rotation,
                Quaternion.LookRotation(Vector3.forward, headingToTarget),
                _novatarConfig.TurnSpeed.AsTimeAdjusted());

            _novatarEntity.SetRotation(rotation);
        }

        private void MoveForward()
        {
            _novatarEntity.Translate(0, _novatarConfig.MovementSpeed.AsTimeAdjusted(), 0);
        }

        private void OnRelationshipSwitched(Pair<RelationshipStatus> relationshipPair)
        {
            var previousIsUnacquainted = relationshipPair.Previous == RelationshipStatus.Unacquainted;
            var currentIsNeutral = relationshipPair.Current == RelationshipStatus.Neutral;

            if (previousIsUnacquainted && currentIsNeutral)
            {
                return;
            }

            var isSilentVisualUpdate = relationshipPair.Current == RelationshipStatus.Unacquainted;
            UpdateLightColor(isSilentVisualUpdate);
        }

        private void UpdateLightColor(bool forceInstant = false)
        {
            var relationship = _novatarStateModel.CurrentRelationshipStatus.Value;
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

        private void SetLight(Color color, float intensity)
        {
            _novatarEntity.HeadLight.color = color;
            _novatarEntity.HeadLight.intensity = intensity;
        }
    }
}
