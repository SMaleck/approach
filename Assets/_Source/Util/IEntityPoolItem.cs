using UnityEngine;

namespace _Source.Util
{
    public interface IEntityPoolItem<T>
    {
        T Entity { get; }
        bool IsFree { get; }
        void Reset(Vector3 spawnPosition);
    }
}
