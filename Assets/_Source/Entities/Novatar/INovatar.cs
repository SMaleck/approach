using UnityEngine;

namespace _Source.Entities.Novatar
{
    public interface INovatar : IMonoEntity
    {
        float SqrRange { get; }
        float SqrTargetReachedThreshold { get; }

        void MoveTowards(Vector3 targetPosition);
        bool IsMovementTargetReached(Vector3 targetPosition);

        float GetSquaredDistanceTo(AbstractMonoEntity otherEntity);
    }
}
