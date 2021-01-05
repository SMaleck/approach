using _Source.Entities.Actors.DataComponents;
using System;
using UniRx;

namespace _Source.Entities.Actors
{
    public interface IActorStateModel
    {
        IDataComponent this[Type type] { get; }
        IOptimizedObservable<Unit> OnReset { get; }
        ActorStateModel Attach(IDataComponent dataComponent);
        void Reset();
    }
}