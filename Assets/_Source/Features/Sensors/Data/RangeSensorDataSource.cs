using _Source.App;
using _Source.Data;
using _Source.Entities.Novatar;
using System;
using UnityEngine;

namespace _Source.Features.Sensors.Data
{
    [Serializable]
    public class RangeSensorDataEntry
    {
        [SerializeField] private EntityState _state;
        public EntityState State => _state;

        [SerializeField] private float _visualRangeColliderSize;
        public float VisualRangeColliderSize => _visualRangeColliderSize;
    }

    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(RangeSensorDataSource), fileName = nameof(RangeSensorDataSource))]
    public class RangeSensorDataSource : AbstractDataSource<RangeSensorDataEntry>
    {
    }
}
