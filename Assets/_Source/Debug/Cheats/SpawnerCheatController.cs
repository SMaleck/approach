using _Source.Features.ActorEntities.Novatar;
using UniRx;
using Zenject;

namespace _Source.Debug.Cheats
{
    public class SpawnerCheatController : AbstractCheatController, IInitializable
    {
        private readonly IDebugNovatarSpawner _novatarSpawner;

        public SpawnerCheatController(IDebugNovatarSpawner novatarSpawner)
        {
            _novatarSpawner = novatarSpawner;
        }

        public void Initialize()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => OnUpdate())
                .AddTo(Disposer);
        }

        private void OnUpdate()
        {
            CheckInput("q", () => _novatarSpawner.Spawn());
            CheckInput("w", () => _novatarSpawner.Pause());
            CheckInput("e", () => _novatarSpawner.Resume());
        }

    }
}
