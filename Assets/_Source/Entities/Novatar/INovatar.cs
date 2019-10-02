namespace _Source.Entities.Novatar
{
    public interface INovatar : IMonoEntity
    {
        void Deactivate();
        void SwitchToEntityState(EntityState entityState);
        
        void TurnLightsOn();
        void ResetIdleTimeouts();
    }
}
