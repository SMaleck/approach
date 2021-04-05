using _Source.Features.Actors.DataComponents;
using _Source.Features.Actors.DataSystems;

namespace _Source.Features.Actors
{
    public interface IActorStateModel
    {
        IActorStateModel Attach(IDataComponent dataComponent);
        IActorStateModel AttachSystem(IDataSystem dataSystem);
        void Reset();

        T Get<T>() where T : class, IDataComponent;
        T[] GetAll<T>() where T : class, IDataComponent;
        bool TryGet<T>(out IDataComponent component) where T : class, IDataComponent;
    }
}