using _Source.Util;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class TimeoutDataComponent : AbstractDataComponent, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<TimeoutDataComponent> { }

        public enum Storage
        {
            Default = 0,
            IdleUnacquainted = 1,
            IdleFriend = 2,
            IdleEnemy = 3,
            WanderTimeout = 4
        }

        private readonly Dictionary<Storage, double> _timesPassed;

        public double this[Storage id]
        {
            get => _timesPassed[id];
            set => _timesPassed[id] = value;
        }

        public TimeoutDataComponent()
        {
            _timesPassed = EnumHelper<Storage>.Iterator
                .ToDictionary(e => e, e => 0d);
        }

        public void Reset()
        {
            _timesPassed.Keys
                .ToArray()
                .ForEach(ResetTimeout);
        }

        public void ResetTimeout(Storage storage)
        {
            _timesPassed[storage] = 0d;
        }

        public void ResetIdleTimeouts()
        {
            Reset();
        }
    }
}
