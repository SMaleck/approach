using System;
using UnityEngine;

namespace _Source.Features.Movement.Data
{
    [Serializable]
    public class WanderDataEntry
    {
        [Range(0, 600)]
        [SerializeField] private double _idleSeconds;
        public double IdleSeconds => _idleSeconds;

        [Range(0, 15)]
        [SerializeField] private float _minDistance;
        public float MinDistance => _minDistance;

        [Range(0, 15)]
        [SerializeField] private float _maxDistance;
        public float MaxDistance => _maxDistance;
    }
}
