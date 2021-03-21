using System;
using UnityEngine;

namespace _Source.Features.Sensors.Data
{
    [Serializable]
    public class RangeSensorDataSource
    {
        [SerializeField] private float _followRange;
        public float FollowRange => _followRange;

        [SerializeField] private float _interactionRange;
        public float InteractionRange => _interactionRange;

        [SerializeField] private float _touchRange;
        public float TouchRange => _touchRange;
    }
}
