using _Source.Features.Actors.DataComponents;

namespace _Source.Features.Actors
{
    public interface IActorStateModel
    {
        ActorStateModel Attach(IDataComponent dataComponent);
        void Reset();

        T Get<T>() where T : class, IDataComponent;
        bool TryGet<T>(out IDataComponent component) where T : class, IDataComponent;
    }
}