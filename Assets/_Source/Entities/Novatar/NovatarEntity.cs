using _Source.Features.NovatarBehaviour;
using _Source.Util;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Novatar
{
    public class NovatarEntity : AbstractMonoEntity, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, NovatarEntity> { }

        [SerializeField] private Light _headLight;

        private NovatarStateModel _novatarStateModel;
        private NovatarConfig _novatarConfig;

        private SerialDisposable _tweenDisposer;

        public float SqrRange => Mathf.Pow(_novatarConfig.Range, 2);
        public float SqrTargetReachedThreshold => Mathf.Pow(_novatarConfig.TargetReachedThreshold, 2);

        [Inject]
        private void Inject(
            NovatarConfig novatarConfig)
        {
            _novatarConfig = novatarConfig;
        }

        // ToDo Quick Hack Zenjectify this properly
        public void Setup(NovatarStateModel novatarStateModel)
        {
            _novatarStateModel = novatarStateModel;
        }

        public void Initialize()
        {
            _novatarStateModel.IsAlive
                .Subscribe(SetActive)
                .AddTo(Disposer);

            _novatarStateModel.SpawnPosition
                .Subscribe(SetPosition)
                .AddTo(Disposer);

            _novatarStateModel.CurrentRelationshipStatus
                .SkipLatestValueOnSubscribe()
                .Subscribe(OnRelationshipChanged)
                .AddTo(Disposer);

            _tweenDisposer = new SerialDisposable().AddTo(Disposer);
        }

        public void Reset()
        {
            UpdateLightColor(true);
        }

        public void MoveTowards(Vector3 targetPosition)
        {
            FaceTarget(targetPosition);
            MoveForward();
        }

        private void FaceTarget(Vector3 targetPosition)
        {
            var headingToTarget = targetPosition - Position;

            if (Vector3.Angle(Position, headingToTarget) < _novatarConfig.TurnAngleThreshold)
            {
                return;
            }

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.forward, headingToTarget),
                _novatarConfig.TurnSpeed.AsTimeAdjusted());
        }

        private void MoveForward()
        {
            transform.Translate(0, _novatarConfig.MovementSpeed.AsTimeAdjusted(), 0);
        }

        private void OnRelationshipChanged(RelationshipStatus relationship)
        {
            UpdateLightColor();
        }

        private void UpdateLightColor(bool forceInstant = false)
        {
            var relationship = _novatarStateModel.CurrentRelationshipStatus.Value;
            var lightColor = _novatarConfig.GetLightColor(relationship);

            if (forceInstant)
            {
                _tweenDisposer.Disposable?.Dispose();
                _headLight.color = lightColor;
                _headLight.intensity = _novatarConfig.LightDefaultIntensity;
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
    }
}
