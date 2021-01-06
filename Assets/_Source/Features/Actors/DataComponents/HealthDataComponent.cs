using _Source.Features.Actors.Data;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class HealthDataComponent : AbstractDisposable, IResettableDataComponent
    {
        public class Factory : PlaceholderFactory<IHealthData, HealthDataComponent> { }

        private readonly IHealthData _healthData;

        private readonly ReactiveProperty<int> _health;
        public IReadOnlyReactiveProperty<int> Health => _health;

        public IReadOnlyReactiveProperty<bool> IsAlive { get; }

        public HealthDataComponent(IHealthData healthData)
        {
            _healthData = healthData;
            _health = new ReactiveProperty<int>(healthData.MaxHealth).AddTo(Disposer);

            IsAlive = _health.Select(health => health > 0)
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposer);
        }

        public void SetHealth(int value)
        {
            _health.Value = value;
        }

        public void SetIsAlive(bool value)
        {
            if (IsAlive.Value && !value)
            {
                _health.Value = 0;
            }
            else if (!IsAlive.Value && value)
            {
                _health.Value = _healthData.MaxHealth;
            }
        }

        public void Reset()
        {
            SetHealth(_healthData.MaxHealth);
        }
    }
}
