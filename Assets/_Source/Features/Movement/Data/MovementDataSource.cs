using _Source.App;
using _Source.Entities.Novatar;
using _Source.Features.Actors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Source.Features.Movement.Data
{
    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(MovementDataSource), fileName = nameof(MovementDataSource))]
    public class MovementDataSource : ScriptableObject
    {
        [Serializable]
        public class StateSpeedFactorRow
        {
            [SerializeField] private EntityState _state;
            public EntityState State => _state;

            [SerializeField] private float _speedFactor;
            public float SpeedFactor => _speedFactor;
        }

        [SerializeField] private EntityType _entity;
        public EntityType Entity => _entity;

        [Header("Movement")]
        [Range(0.1f, 10)]
        [SerializeField] private float _movementSpeed;
        public float MovementSpeed => _movementSpeed;

        [SerializeField] private float _movementDeadZoneMagnitude;
        public float MovementDeadZoneMagnitude => _movementDeadZoneMagnitude;

        [Header("Turning")]
        [Range(0.1f, 50)]
        [SerializeField] private float _turnSpeed;
        public float TurnSpeed => _turnSpeed;

        [SerializeField] private float _turnDeadZoneAngle;
        public float TurnDeadZoneAngle => _turnDeadZoneAngle;

        [Header("Misc")]
        [SerializeField] private bool _useDirectMovement;
        public bool UseDirectMovement => _useDirectMovement;

        [Range(0.001f, 5)]
        [SerializeField] private float _moveTargetReachedAccuracy;
        public float MoveTargetReachedAccuracy => _moveTargetReachedAccuracy;

        [Range(0.001f, 5)]
        [SerializeField] private float _moveTargetTouchedAccuracy;
        public float MoveTargetTouchedAccuracy => _moveTargetTouchedAccuracy;

        [SerializeField] private List<StateSpeedFactorRow> _stateSpeedFactors;
        public List<StateSpeedFactorRow> StateSpeedFactors => _stateSpeedFactors;
    }
}
