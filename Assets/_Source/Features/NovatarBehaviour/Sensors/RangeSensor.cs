using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Source.Entities;
using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Sensors
{
    public class RangeSensor
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
            var isInRange = _sqrDistanceToAvatar <= _novatarEntity.SqrRange;
            return isInRange && !IsInTouchRange();
        }

        public bool IsInTouchRange()
        {
            return _sqrDistanceToAvatar <= _novatarEntity.SqrTargetReachedThreshold;
        }
    }
}
