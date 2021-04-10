using _Source.App;
using _Source.Debug.Cheats;
using UnityEngine;
using Zenject;

namespace _Source.Debug.Installation
{
    [CreateAssetMenu(fileName = nameof(FeatureDebugInstaller), menuName = Constants.InstallersMenu + nameof(FeatureDebugInstaller))]
    public class FeatureDebugInstaller : ScriptableObjectInstaller<FeatureDebugInstaller>
    {
        [SerializeField] private RoundStatsDebugView _roundStatsDebugView;

        public override void InstallBindings()
        {
            if (!UnityEngine.Debug.isDebugBuild)
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
