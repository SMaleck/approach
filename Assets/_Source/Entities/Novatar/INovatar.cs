using UnityEngine;

namespace _Source.Entities.Novatar
{
    public interface INovatar : IMonoEntity
    {
        float SqrRange { get; }
        float SqrTargetReachedThreshold { get; }

        void SwitchToEntityState(EntityState entityState);
        void SetCurrentDistanceToAvatar(float value);
        void Deactivate();

        float GetSquaredDistanceTo(IMonoEntity otherEntity);

        void TurnLightsOn();
    }
}
