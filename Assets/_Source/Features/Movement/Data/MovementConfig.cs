using System;
using UnityEngine;

namespace _Source.Features.Movement.Data
{
    [Serializable]
    public class MovementConfig : IMovementData
    {
        [SerializeField] private float _movementSpeed;
        public float MovementSpeed => _movementSpeed;

        [SerializeField] private float _movementDeadZoneMagnitude;
        public float MovementDeadZoneMagnitude => _movementDeadZoneMagnitude;

        [SerializeField] private float _turnSpeed;
        public float TurnSpeed => _turnSpeed;

        [SerializeField] private float _turnDeadZoneAngle;
        public float TurnDeadZoneAngle => _turnDeadZoneAngle;
    }
}
