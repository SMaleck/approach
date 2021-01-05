using _Source.Entities.Actors.DataComponents;
using System;
using UniRx;

namespace _Source.Entities.Actors
{
    public interface IActorStateModel
    {
        IDataComponent this[Type type] { get; }
        IObservable<Unit> OnReset { get; }
        IObservable<Unit> OnResetIdleTimeouts { get; }

        ActorStateModel Attach(IDataComponent dataComponent);
        T Get<T>() where T : class, IDataComponent;

        void Reset();
        void PublishOnResetIdleTimeouts();
    }
}