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
        public IOptimizedObservable<Unit> OnReset => _onReset;

        public ActorStateModel()
        {
            _dataComponents = new Dictionary<Type, IDataComponent>();
            _resettableDataComponents = new List<IResettableDataComponent>();
            _onReset = new Subject<Unit>().AddTo(Disposer);
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

        public void Reset()
        {
            _resettableDataComponents.ForEach(e => e.Reset());
            _onReset.OnNext(Unit.Default);
        }
    }
}
