using _Source.Entities;
using _Source.Features.ActorSensors.Data;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorSensors
{
    public class RangeSensor : ISensor
    {
        public class Factory : PlaceholderFactory<IRangeSensorData, IMonoEntity, IMonoEntity, RangeSensor> { }

        private readonly IMonoEntity _ownerEntity;
        private readonly IMonoEntity _targetEntity;

        private readonly float _sqrFollowRange;
        private readonly float _sqrInteractionRange;
        private readonly float _sqrTouchRange;
        private float _sqrDistanceToAvatar;

        public RangeSensor(
            IRangeSensorData data,
            IMonoEntity ownerEntity,
            IMonoEntity targetEntity)
        {
            _ownerEntity = ownerEntity;
            _targetEntity = targetEntity;

            _sqrFollowRange = Mathf.Pow(data.FollowRange, 2);
            _sqrInteractionRange = Mathf.Pow(data.InteractionRange, 2);
            _sqrTouchRange = Mathf.Pow(data.TouchRange, 2);
        }

        public void UpdateSensorReadings()
        {
            var distance = _ownerEntity.Position - _targetEntity.Position;
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
            return _targetEntity.Position;
        }
    }
}
