using _Source.App;
using _Source.Entities;
using _Source.Features.GameWorld.Data;
using _Source.Installation.Data;
using UnityEngine;
using Zenject;

namespace _Source.Installation
{
    [CreateAssetMenu(fileName = nameof(DataInstaller), menuName = Constants.ConfigRootPath + "/" + nameof(DataInstaller))]
    public class DataInstaller : ScriptableObjectInstaller<DataInstaller>
    {
        [SerializeField] private AvatarConfig _avatarConfig;
        [SerializeField] private NovatarConfig _novatarConfig;
        [SerializeField] private ViewPrefabsConfig _viewPrefabsConfig;
        [SerializeField] private NovatarSpawnerConfig _novatarSpawnerConfig;

        public override void InstallBindings()
        {
            Container.BindInstances(_avatarConfig);
            Container.BindInstance(_novatarConfig);
            Container.BindInstances(_viewPrefabsConfig);
            Container.BindInstances(_novatarSpawnerConfig);
        }
    }
}
