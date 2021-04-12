using _Source.App;
using _Source.Debug.Cheats;
using _Source.Services.Environment;
using UnityEngine;
using Zenject;

namespace _Source.Debug.Installation
{
    [CreateAssetMenu(fileName = nameof(FeatureDebugInstaller), menuName = Constants.InstallersMenu + nameof(FeatureDebugInstaller))]
    public class FeatureDebugInstaller : ScriptableObjectInstaller<FeatureDebugInstaller>
    {
        [SerializeField] private RoundStatsDebugView _roundStatsDebugView;

        [Inject] private readonly IEnvironmentService _environmentService;

        public override void InstallBindings()
        {
            if (!_environmentService.IsDebug)
            {
                return;
            }

            Container.BindInterfacesTo<SpawnerCheatController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<RoundCheatController>().AsSingle().NonLazy();

            Container.Bind<RoundStatsDebugView>()
                .FromComponentInNewPrefab(_roundStatsDebugView)
                .AsSingle()
                .NonLazy();
        }
    }
}
