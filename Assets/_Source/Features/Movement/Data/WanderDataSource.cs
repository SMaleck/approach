using _Source.App;
using _Source.Data;
using System;
using UnityEngine;

namespace _Source.Features.Movement.Data
{
    [Serializable]
    public class WanderDataEntry
    {
        [Range(0, 15)]
        [SerializeField] private float _minDistance;
        public float MinDistance => _minDistance;

        [Range(0, 15)]
        [SerializeField] private float _maxDistance;
        public float MaxDistance => _maxDistance;
    }

    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(WanderDataSource), fileName = nameof(WanderDataSource))]
    public class WanderDataSource : AbstractDataSource<WanderDataEntry>
    {
    }
}
