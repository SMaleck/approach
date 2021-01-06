using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ActorSensors.Data;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorSensors
{
    public class RangeSensor : ISensor
    {
        public class Factory : PlaceholderFactory<IActorStateModel, IActorStateModel, IRangeSensorData, RangeSensor> { }

        private readonly TransformDataComponent _ownerTransformData;
        private readonly TransformDataComponent _targetTransformData;

        private readonly float _sqrFollowRange;
        private readonly float _sqrInteractionRange;
        private readonly float _sqrTouchRange;
        private float _sqrDistanceToAvatar;

        public RangeSensor(
            IActorStateModel ownerActor,
            IActorStateModel targetActor,
            IRangeSensorData data)
        {
            _ownerTransformData = ownerActor.Get<TransformDataComponent>();
            _targetTransformData = targetActor.Get<TransformDataComponent>();

            _sqrFollowRange = Mathf.Pow(data.FollowRange, 2);
            _sqrInteractionRange = Mathf.Pow(data.InteractionRange, 2);
            _sqrTouchRange = Mathf.Pow(data.TouchRange, 2);
        }

        public void UpdateSensorReadings()
        {
            var distance = _ownerTransformData.Position - _targetTransformData.Position;
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
            return _targetTransformData.Position;
        }
    }
}
