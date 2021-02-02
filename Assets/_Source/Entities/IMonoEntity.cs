using _Source.Features.Actors;
using UnityEngine;

namespace _Source.Entities
{
    public interface IMonoEntity
    {
        string Name { get; }
        bool IsActive { get; }

        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 Size { get; }

        Transform LocomotionTarget { get; }
        Transform RotationTarget { get; }

        IActorStateModel ActorStateModel { get; }

        void Setup(IActorStateModel actorStateModel);
        string ToDebugString();
    }
}
