using _Source.Util;
using Packages.SavegameSystem.Models;
using System;
using UniRx;

namespace _Source.Services.SavegameSystem.Models
{
    public class AbstractSavegame : AbstractDisposable, ISavegame
    {
        protected static ReactiveProperty<T> CreateBoundProperty<T>(
            T initialValue,
            Action<T> setter,
            CompositeDisposable disposer)
        {
            var rxProperty = new ReactiveProperty<T>(initialValue).AddTo(disposer);
            rxProperty.Subscribe(setter).AddTo(disposer);

            return rxProperty;
        }
    }
}
