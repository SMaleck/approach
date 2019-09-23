using System;
using UnityEngine;

namespace _Source.Features.Movement.Data
{
    [Serializable]
    public class MovementConfig : IMovementData
    {
        [SerializeField] private bool _useDirectMovement;
        public bool UseDirectMovement => _useDirectMovement;

        [Range(0.001f, 5)]
        [SerializeField] private float _moveTargetReachedAccuracy;
        public float MoveTargetReachedAccuracy => _moveTargetReachedAccuracy;

        [Range(0.001f, 5)]
        [SerializeField] private float _moveTargetTouchedAccuracy;
        public float MoveTargetTouchedAccuracy => _moveTargetTouchedAccuracy;

        [Range(0.1f, 10)]
        [SerializeField] private float _movementSpeed;
        public float MovementSpeed => _movementSpeed;

        [SerializeField] private float _movementDeadZoneMagnitude;
        public float MovementDeadZoneMagnitude => _movementDeadZoneMagnitude;

        [Range(0.1f, 10)]
        [SerializeField] private float _turnSpeed;
        public float TurnSpeed => _turnSpeed;

        [SerializeField] private float _turnDeadZoneAngle;
        public float TurnDeadZoneAngle => _turnDeadZoneAngle;
    }
}
