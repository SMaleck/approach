using _Source.Entities.Actors.DataComponents;
using _Source.Util;
using System;
using System.Collections.Generic;
using UniRx;
using Zenject;

namespace _Source.Entities.Actors
{
    public class ActorStateModel : AbstractDisposableFeature
    {
        public class Factory : PlaceholderFactory<ActorStateModel> { }

        private readonly Dictionary<Type, IDataComponent> _dataComponents;

        public IDataComponent this[Type type] => _dataComponents[type];

        private readonly Subject<Unit> _onReset;
        public IOptimizedObservable<Unit> OnReset => _onReset;

        public ActorStateModel()
        {
            _dataComponents = new Dictionary<Type, IDataComponent>();
        }

        public void Attach(IDataComponent dataComponent)
        {
            _dataComponents.Add(dataComponent.GetType(), dataComponent);
            dataComponent.AddTo(Disposer);
        }

        public void Reset()
        {
            _onReset.OnNext(Unit.Default);
        }
    }
}
