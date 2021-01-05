using System;
using _Source.Features.Actors.DataComponents;
using UniRx;

namespace _Source.Features.Actors
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