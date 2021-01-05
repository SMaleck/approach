using _Source.App;
using _Source.Features.ActorEntities.Avatar.Data;
using _Source.Features.ActorEntities.Novatar.Data;
using UnityEngine;
using Zenject;

namespace _Source.Installation
{
    [CreateAssetMenu(fileName = nameof(DataSourceInstaller), menuName = Constants.InstallersMenu + nameof(DataSourceInstaller))]
    public class DataSourceInstaller : ScriptableObjectInstaller<DataSourceInstaller>
    {
        [SerializeField] private AvatarDataSource _avatarDataSource;
        [SerializeField] private NovatarDataSource _novatarDataSource;

        public override void InstallBindings()
        {
            Container.BindInstance(_avatarDataSource);
            Container.BindInstance(_novatarDataSource);
        }
    }
}
