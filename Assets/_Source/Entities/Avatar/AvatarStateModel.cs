using _Source.Util;
using System;
using UniRx;

namespace _Source.Entities.Avatar
{
    public class AvatarStateModel : AbstractDisposable, IAvatarStateModel
    {
        private readonly ReactiveProperty<DateTime> _startedAt;
        public IReadOnlyReactiveProperty<DateTime> StartedAt => _startedAt;

        private readonly ReactiveProperty<double> _survivalSeconds;
        public IReadOnlyReactiveProperty<double> SurvivalSeconds => _survivalSeconds;

        public IReadOnlyReactiveProperty<bool> IsAlive { get; }

        private readonly ReactiveProperty<double> _health;
        public IReadOnlyReactiveProperty<double> Health => _health;

        public AvatarStateModel(AvatarConfig avatarConfig)
        {
            _startedAt = new ReactiveProperty<DateTime>().AddTo(Disposer);
            _survivalSeconds = new ReactiveProperty<double>().AddTo(Disposer);
            _health = new ReactiveProperty<double>(avatarConfig.Health).AddTo(Disposer);

            IsAlive = _health.Select(health => health > 0)
                .ToReadOnlyReactiveProperty()
                .AddTo(Disposer);
        }

        public void SetStartedAt(DateTime value)
        {
            _startedAt.Value = value;
        }

        public void SetSurvivalSeconds(double value)
        {
            _survivalSeconds.Value = value;
        }

        public void SetHealth(double value)
        {
            _health.Value = value;
        }
    }
}
