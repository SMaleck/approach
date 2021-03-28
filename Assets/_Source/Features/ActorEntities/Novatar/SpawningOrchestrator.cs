using _Source.Util;
using System;
using _Source.Features.ActorEntities.Config;
using UniRx;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    public class SpawningOrchestrator : AbstractDisposable, IInitializable, IDebugNovatarSpawner
    {
        private readonly NovatarSpawnerConfig _novatarSpawnerConfig;
        private readonly NovatarSpawner _novatarSpawner;

        private bool _isEnabled = true;

        public SpawningOrchestrator(
            NovatarSpawnerConfig novatarSpawnerConfig,
            NovatarSpawner novatarSpawner)
        {
            _novatarSpawnerConfig = novatarSpawnerConfig;
            _novatarSpawner = novatarSpawner;
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

            return _isEnabled &&
                   activeCount < _novatarSpawnerConfig.MaxActiveSpawns;
        }

        void IDebugNovatarSpawner.Spawn()
        {
            _novatarSpawner.Spawn();
        }

        void IDebugNovatarSpawner.Pause()
        {
            _isEnabled = false;
        }

        void IDebugNovatarSpawner.Resume()
        {
            _isEnabled = true;
        }
    }
}
