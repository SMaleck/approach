namespace _Source.Features.ActorEntities.Components
{
    public interface IMonoComponent
    {
        void Setup(IMonoEntity entity);
        void StartLifeCycle();
        void StopLifeCycle();
    }
}
