using _Source.App;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using System;
using System.Collections.Generic;
using UniRx;
using Zenject;

namespace _Source.Features.Actors
{
    public class ActorStateModel : AbstractDisposableFeature, IActorStateModel
    {
        public class Factory : PlaceholderFactory<ActorStateModel> { }

        private readonly Dictionary<Type, IDataComponent> _dataComponents;
        private readonly List<IResettableDataComponent> _resettableDataComponents;

        public ActorStateModel()
        {
            _dataComponents = new Dictionary<Type, IDataComponent>();
            _resettableDataComponents = new List<IResettableDataComponent>();
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
            if (TryGet<T>(out var component))
            {
                return component as T;
            }

            Logger.Warn($"No DataComponent of type [{(typeof(T).Name)}] found");
            return null;
        }

        public bool TryGet<T>(out IDataComponent component) where T : class, IDataComponent
        {
            return _dataComponents.TryGetValue(typeof(T), out component);
        }

        public void Reset()
        {
            _resettableDataComponents.ForEach(e => e.Reset());
        }
    }
}
