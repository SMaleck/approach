namespace _Source.Features.Movement.Data
{
    public interface IWanderData
    {
        double IdleSeconds { get; }
        float MinDistance { get; }
        float MaxDistance { get; }
    }
}
