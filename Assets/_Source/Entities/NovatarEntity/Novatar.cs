using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities
{
    public class Novatar : AbstractMonoEntity, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, Novatar> { }

        private NovatarConfig _novatarConfig;
        private Avatar _avatar;

        private float sqrRange;
        private float sqrTargetReachedThreshold;

        [Inject]
        private void Inject(
            NovatarConfig novatarConfig,
            Avatar avatar)
        {
            _novatarConfig = novatarConfig;
            _avatar = avatar;

            sqrRange = Mathf.Pow(_novatarConfig.Range, 2);
            sqrTargetReachedThreshold = Mathf.Pow(_novatarConfig.TargetReachedThreshold, 2);
        }

        public void Initialize()
        {
            Observable.EveryLateUpdate()
                .Subscribe(_ => OnLateUpdate())
                .AddTo(Disposer);
        }

        private void OnLateUpdate()
        {
            var heading = _avatar.HeadingTo(this);
            var sqrDistance = heading.sqrMagnitude;

            var isInRange = sqrDistance <= sqrRange;
            var isTouching = sqrDistance <= sqrTargetReachedThreshold;

            if (!isInRange || isTouching)
            {
                return;
            }

            Follow();
        }

        private void Follow()
        {
            FaceTarget();
            MoveForward();
        }

        private void FaceTarget()
        {
            var headingToTarget = _avatar.Position - Position;

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
    }
}
