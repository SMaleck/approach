using _Source.App;
using _Source.Features.ActorEntities.Avatar.Data;
using _Source.Features.ActorEntities.Novatar.Data;
using _Source.Features.GameRound.Data;
using _Source.Features.Movement.Data;
using _Source.Features.Sensors.Data;
using UnityEngine;
using Zenject;

namespace _Source.Installation
{
    [CreateAssetMenu(fileName = nameof(DataSourceInstaller), menuName = Constants.InstallersMenu + nameof(DataSourceInstaller))]
    public class DataSourceInstaller : ScriptableObjectInstaller<DataSourceInstaller>
    {
        [Header("Avatar")] [SerializeField] private AvatarDataSource _avatarDataSource;
        [SerializeField] private MovementDataSource _avatarMovementDataSource;

        [Header("Novatar")] [SerializeField] private NovatarDataSource _novatarDataSource;
        [SerializeField] private MovementDataSource _novatarMovementDataSource;

        [SerializeField] private RangeSensorDataSource _rangeSensorDataSource;
        [SerializeField] private WanderDataSource _wanderDataSource;
        [SerializeField] private GameRoundDataSource _gameRoundDataSource;

        public override void InstallBindings()
        {
            Container.BindInstance(_avatarDataSource);
            Container.BindInstance(_avatarMovementDataSource);
            Container.BindInstance(_novatarDataSource);
            Container.BindInstance(_novatarMovementDataSource);
            Container.BindInstance(_rangeSensorDataSource);
            Container.BindInstance(_wanderDataSource);
            Container.BindInstance(_gameRoundDataSource);
        }
    }
}
