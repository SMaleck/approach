namespace _Source.Features.Actors.Data
{
    public interface ISensorData
    {
        float FollowRange { get; }
        float InteractionRange { get; }
        float TouchRange { get; }
    }
}
