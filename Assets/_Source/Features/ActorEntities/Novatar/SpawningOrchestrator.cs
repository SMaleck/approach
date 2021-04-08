using _Source.Features.ActorEntities.Config;
using _Source.Features.FeatureToggles;
using _Source.Util;
using System;
using UniRx;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    public class SpawningOrchestrator : AbstractDisposable, IInitializable, IDebugNovatarSpawner, ITogglableFeature
    {
        public FeatureId FeatureId => FeatureId.NovatarSpawning;

        private readonly NovatarSpawnerConfig _novatarSpawnerConfig;
        private readonly NovatarSpawner _novatarSpawner;

        private readonly IReactiveProperty<bool> _isEnabled;
        public IReadOnlyReactiveProperty<bool> IsEnabled => _isEnabled;

        public SpawningOrchestrator(
            NovatarSpawnerConfig novatarSpawnerConfig,
            NovatarSpawner novatarSpawner)
        {
            _novatarSpawnerConfig = novatarSpawnerConfig;
            _novatarSpawner = novatarSpawner;

            _isEnabled = new ReactiveProperty<bool>(true).AddTo(Disposer);
        }

        public void Initialize()
        {
            Observable.Interval(TimeSpan.FromSeconds(_novatarSpawnerConfig.SpawnIntervalSeconds))
                .Where(_ => CanSpawn())
                .Subscribe(_ => _novatarSpawner.Spawn())
                .AddTo(Disposer);
        }

        private bool CanSpawn()
        {
            var activeCount = _novatarSpawner.GetActiveNovatarCount();

            return IsEnabled.Value &&
                   activeCount < _novatarSpawnerConfig.MaxActiveSpawns;
        }

        void IDebugNovatarSpawner.Spawn()
        {
            _novatarSpawner.Spawn();
        }

        void IDebugNovatarSpawner.Pause()
        {
            SetIsEnabled(false);
        }

        void IDebugNovatarSpawner.Resume()
        {
            SetIsEnabled(true);
        }

        public void SetIsEnabled(bool isEnabled)
        {
            _isEnabled.Value = isEnabled;
        }
    }
}
