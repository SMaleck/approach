using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour;
using UnityEngine;

namespace _Source.Features.NovatarSpawning
{
    public interface IEntityPoolItem
    {
        bool IsFree { get; }

        NovatarEntity NovatarEntity { get; }
        NovatarStateModel NovatarStateModel { get; }

        void Reset(Vector3 spawnPosition);
    }
}
