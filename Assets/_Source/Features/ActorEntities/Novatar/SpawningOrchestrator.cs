using System;
using _Source.Features.ActorEntities.Novatar.Config;
using _Source.Util;
using UniRx;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    public class SpawningOrchestrator : AbstractDisposable, IInitializable
    {
        private readonly NovatarSpawnerConfig _novatarSpawnerConfig;
        private readonly NovatarSpawner _novatarSpawner;

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
            return activeCount < _novatarSpawnerConfig.MaxActiveSpawns;
        }
    }
}
