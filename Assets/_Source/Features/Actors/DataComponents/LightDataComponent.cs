using _Source.Util;
using System;
using UniRx;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class LightDataComponent : AbstractDisposable, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<LightDataComponent> { }

        private readonly IReactiveProperty<bool> _isOn;
        public IReadOnlyReactiveProperty<bool> IsOn => _isOn;

        private readonly ISubject<Unit> _onLightsSwitchedOn;
        public IObservable<Unit> OnLightsSwitchedOn => _onLightsSwitchedOn;

        public LightDataComponent()
        {
            _isOn = new ReactiveProperty<bool>().AddTo(Disposer);
            _onLightsSwitchedOn = new Subject<Unit>().AddTo(Disposer);
        }

        public void TurnLightsOn()
        {
            _isOn.Value = true;
            _onLightsSwitchedOn.OnNext(Unit.Default);
        }

        public void Reset()
        {
            _isOn.Value = false;
        }
    }
}
