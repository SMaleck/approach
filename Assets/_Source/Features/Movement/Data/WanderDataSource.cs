using System;
using UnityEngine;

namespace _Source.Features.Movement.Data
{
    [Serializable]
    public class WanderDataSource
    {
        [Range(-50, 50)]
        [SerializeField] private float _wanderMinDistance;
        public float WanderMinDistance => _wanderMinDistance;

        [Range(-50, 50)]
        [SerializeField] private float _wanderMaxDistance;
        public float WanderMaxDistance => _wanderMaxDistance;
    }
}
