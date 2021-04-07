using _Source.Util;
using Packages.SavegameSystem.Models;
using System;
using UniRx;

namespace _Source.Services.SavegameSystem.Models
{
    public abstract class AbstractSavegame : AbstractDisposable, ISavegame
    {
        protected IReactiveProperty<T> CreateBoundProperty<T>(
            T initialValue,
            Action<T> setter)
        {
            var rxProperty = new ReactiveProperty<T>(initialValue).AddTo(Disposer);
            rxProperty.Subscribe(setter).AddTo(Disposer);

            return rxProperty;
        }
    }
}
