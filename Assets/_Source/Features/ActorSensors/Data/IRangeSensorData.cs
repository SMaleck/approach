namespace _Source.Features.ActorSensors.Data
{
    public interface IRangeSensorData
    {
        float FollowRange { get; }
        float InteractionRange { get; }
        float TouchRange { get; }
    }
}
