using UnityEngine;

namespace _Source.Features.Movement
{
    public interface IMovableEntity
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }

        Transform LocomotionTarget { get; }
        Transform RotationTarget { get; }
    }
}
