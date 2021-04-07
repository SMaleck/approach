using _Source.Features.Actors.Data;
using System;
using UniRx;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class HealthDataComponent : AbstractDataComponent, IResettableDataComponent, IDamageReceiver
    {
        public class Factory : PlaceholderFactory<IHealthData, HealthDataComponent> { }

        private readonly IHealthData _healthData;

        public int MaxHealth => _healthData.MaxHealth;

        private readonly ReactiveProperty<int> _health;
        public IReadOnlyReactiveProperty<int> Health => _health;

        public IReadOnlyReactiveProperty<double> RelativeHealth { get; }
        public IReadOnlyReactiveProperty<bool> IsAlive { get; }

        public HealthDataComponent(IHealthData healthData)
        {
            _healthData = healthData;
            _health = new ReactiveProperty<int>(MaxHealth).AddTo(Disposer);

            RelativeHealth = _health
                .Select(health => (double)health / (double)MaxHealth)
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposer);

            IsAlive = _health
                .Select(health => health > 0)
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposer);
        }

        public void SetHealth(int value)
        {
            _health.Value = Math.Max(0, value);
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
            SetHealth(MaxHealth);
        }

        public void ReceiveDamage(int damageAmount)
        {
            SetHealth(_health.Value - damageAmount);
        }
    }
}
