using UnityEngine;

namespace _Source.Entities.Novatar
{
    public interface INovatar : IMonoEntity
    {
        float SqrRange { get; }
        float SqrTargetReachedThreshold { get; }

        void MoveTowards(Vector3 targetPosition);
        void MoveForward();
        bool IsMovementTargetReached(Vector3 targetPosition);
        void LookAt(Vector3 targetPosition);

        float GetSquaredDistanceTo(AbstractMonoEntity otherEntity);

        void TurnLightsOn();
    }
}
