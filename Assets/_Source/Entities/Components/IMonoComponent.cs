namespace _Source.Entities.Components
{
    public interface IMonoComponent
    {
        void Setup(IMonoEntity entity);
        void StartLifeCycle();
        void StopLifeCycle();
    }
}
