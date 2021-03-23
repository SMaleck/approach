using _Source.Features.Actors.DataComponents;
using System;
using UniRx;

namespace _Source.Features.Actors
{
    public interface IActorStateModel
    {
        IObservable<Unit> OnReset { get; }
        IObservable<Unit> OnResetIdleTimeouts { get; }

        ActorStateModel Attach(IDataComponent dataComponent);

        T Get<T>() where T : class, IDataComponent;
        bool TryGet<T>(out IDataComponent component) where T : class, IDataComponent;

        void Reset();
        void ResetIdleTimeouts();
    }
}