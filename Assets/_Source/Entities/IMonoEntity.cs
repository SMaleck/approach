using UnityEngine;

namespace _Source.Entities
{
    public interface IMonoEntity
    {
        bool IsActive { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 Size { get; }
    }
}
