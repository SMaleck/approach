using _Source.Entities.Avatar;
using _Source.Features.NovatarBehaviour;
using _Source.Util;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Novatar
{
    public class NovatarEntity : AbstractMonoEntity
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, NovatarEntity> { }

        private NovatarStateModel _novatarStateModel;
        private NovatarConfig _novatarConfig;

        public Vector3 SpawnedAtPosition { get; private set; }
        public float SqrRange => Mathf.Pow(_novatarConfig.Range, 2);
        public float SqrTargetReachedThreshold => Mathf.Pow(_novatarConfig.TargetReachedThreshold, 2);
        
        [Inject]
        private void Inject(
            NovatarConfig novatarConfig)
        {
            _novatarConfig = novatarConfig;
        }

        // ToDo Quick Hack Zenjectify this properly
        public void Setup(
            NovatarStateModel novatarStateModel,
            Vector3 spawnPosition)
        {
            _novatarStateModel = novatarStateModel;

            SpawnedAtPosition = spawnPosition;
            SetPosition(spawnPosition);
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
    }
}
