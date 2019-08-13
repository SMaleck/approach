using _Source.Util;
using System;
using UniRx;

namespace _Source.Features.AvatarState
{
    public class SurvivalStatsController : AbstractDisposable, IDamageReceiver
    {
        private readonly AvatarStateModel _avatarStateModel;

        public SurvivalStatsController(AvatarStateModel avatarStateModel)
        {
            _avatarStateModel = avatarStateModel;

            _avatarStateModel.SetStartedAt(DateTime.Now);

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ => OnTimePassed())
                .AddTo(Disposer);
        }

        private void OnTimePassed()
        {
            var timePassed = DateTime.Now - _avatarStateModel.StartedAt.Value;
            _avatarStateModel.SetSurvivalSeconds(timePassed.TotalSeconds);
        }

        public void ReceiveDamage(double damageAmount)
        {
            var currentHealth = _avatarStateModel.Health.Value;
            var newHealth = Math.Max(0, currentHealth - damageAmount);

            _avatarStateModel.SetHealth(newHealth);
        }
    }
}
