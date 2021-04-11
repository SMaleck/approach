using UnityEngine;

namespace _Source.Features.ActorEntities.Novatar
{
    public interface INovatarPoolItem
    {
        bool IsFree { get; }
        bool IsFriend { get; }
        Vector3 Size { get; }

        void Reset(Vector3 spawnPosition);
    }
}
