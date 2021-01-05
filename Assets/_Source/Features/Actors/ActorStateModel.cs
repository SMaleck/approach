using _Source.Entities.Actors.DataComponents;
using _Source.Util;
using System;
using System.Collections.Generic;
using UniRx;
using Zenject;

namespace _Source.Entities.Actors
{
    public class ActorStateModel : AbstractDisposableFeature, IActorStateModel
    {
        public class Factory : PlaceholderFactory<ActorStateModel> { }

        private readonly Dictionary<Type, IDataComponent> _dataComponents;
        private readonly List<IResettableDataComponent> _resettableDataComponents;

        public IDataComponent this[Type type] => _dataComponents[type];

        private readonly Subject<Unit> _onReset;
        public IObservable<Unit> OnReset => _onReset;

        private readonly Subject<Unit> _onResetIdleTimeouts;
        public IObservable<Unit> OnResetIdleTimeouts => _onResetIdleTimeouts;

        public ActorStateModel()
        {
            _dataComponents = new Dictionary<Type, IDataComponent>();
            _resettableDataComponents = new List<IResettableDataComponent>();
            _onReset = new Subject<Unit>().AddTo(Disposer);
            _onResetIdleTimeouts = new Subject<Unit>().AddTo(Disposer);
        }

        public ActorStateModel Attach(IDataComponent dataComponent)
        {
            _dataComponents.Add(dataComponent.GetType(), dataComponent);
            dataComponent.AddTo(Disposer);

            if (dataComponent is IResettableDataComponent resettable)
            {
                _resettableDataComponents.Add(resettable);
            }

            return this;
        }

        public T Get<T>() where T : class, IDataComponent
        {
            return this[typeof(T)] as T;
        }

        public void Reset()
        {
            _resettableDataComponents.ForEach(e => e.Reset());
            _onReset.OnNext(Unit.Default);
        }

        public void PublishOnResetIdleTimeouts()
        {
            _onResetIdleTimeouts.OnNext(Unit.Default);
        }
    }
}
