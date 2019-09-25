namespace _Source.Entities.Novatar
{
    public interface INovatar : IMonoEntity
    {
        float SqrRange { get; }
        float SqrTargetReachedThreshold { get; }

        void Deactivate();
        void SwitchToEntityState(EntityState entityState);
        
        void TurnLightsOn();
        void ResetIdleTimeouts();
    }
}
