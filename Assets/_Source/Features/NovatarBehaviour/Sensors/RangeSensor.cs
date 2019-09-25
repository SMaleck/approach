using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Sensors
{
    public class RangeSensor : ISensor
    {
        public class Factory : PlaceholderFactory<INovatar, RangeSensor> { }

        private readonly INovatar _novatarEntity;
        private readonly IAvatar _avatarEntity;

        private float _sqrDistanceToAvatar;

        public RangeSensor(
            INovatar novatarEntity,
            IAvatar avatarEntity)
        {
            _novatarEntity = novatarEntity;
            _avatarEntity = avatarEntity;
        }

        public void UpdateSensorReadings()
        {
            var distance = _novatarEntity.Position - _avatarEntity.Position;
            _sqrDistanceToAvatar = distance.sqrMagnitude;
        }

        public bool IsInFollowRange()
        {
            return _sqrDistanceToAvatar <= _novatarEntity.SqrRange;
        }

        public bool IsInTouchRange()
        {
            return _sqrDistanceToAvatar <= _novatarEntity.SqrTargetReachedThreshold;
        }

        public Vector3 GetAvatarPosition()
        {
            return _avatarEntity.Position;
        }
    }
}
