using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour.Sensors.Data;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Sensors
{
    public class RangeSensor : ISensor
    {
        public class Factory : PlaceholderFactory<INovatar, RangeSensorConfig, RangeSensor> { }

        private readonly INovatar _novatarEntity;
        private readonly IAvatar _avatarEntity;

        private readonly float _sqrFollowRange;
        private readonly float _sqrInteractionRange;
        private readonly float _sqrTouchRange;
        private float _sqrDistanceToAvatar;
        
        public RangeSensor(
            INovatar novatarEntity,
            RangeSensorConfig rangeSensorConfig,
            IAvatar avatarEntity)
        {
            _novatarEntity = novatarEntity;
            _avatarEntity = avatarEntity;

            _sqrFollowRange = Mathf.Pow(rangeSensorConfig.FollowRange, 2);
            _sqrInteractionRange = Mathf.Pow(rangeSensorConfig.InteractionRange, 2);
            _sqrTouchRange = Mathf.Pow(rangeSensorConfig.TouchRange, 2);
        }

        public void UpdateSensorReadings()
        {
            var distance = _novatarEntity.Position - _avatarEntity.Position;
            _sqrDistanceToAvatar = distance.sqrMagnitude;
        }

        public bool IsInFollowRange()
        {
            return _sqrDistanceToAvatar <= _sqrFollowRange;
        }

        public bool IsInInteractionRange()
        {
            return _sqrDistanceToAvatar <= _sqrInteractionRange;
        }

        public bool IsInTouchRange()
        {
            return _sqrDistanceToAvatar <= _sqrTouchRange;
        }

        public Vector3 GetAvatarPosition()
        {
            return _avatarEntity.Position;
        }
    }
}
