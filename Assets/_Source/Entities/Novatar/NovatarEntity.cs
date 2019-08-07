using _Source.Entities.Avatar;
using _Source.Util;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Novatar
{
    public class NovatarEntity : AbstractMonoEntity, IInitializable
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, NovatarEntity> { }

        private NovatarConfig _novatarConfig;
        private AvatarEntity _avatar;

        public float SqrRange => Mathf.Pow(_novatarConfig.Range, 2);
        public float SqrTargetReachedThreshold => Mathf.Pow(_novatarConfig.TargetReachedThreshold, 2);

        [Inject]
        private void Inject(
            NovatarConfig novatarConfig,
            AvatarEntity avatar)
        {
            _novatarConfig = novatarConfig;
            _avatar = avatar;
        }

        public void Initialize()
        {
        }

        public void FollowAvatar()
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
