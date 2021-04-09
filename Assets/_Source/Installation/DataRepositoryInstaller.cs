using _Source.Features.ActorBehaviours.Data;
using _Source.Features.ActorEntities.Avatar.Data;
using _Source.Features.ActorEntities.Novatar.Data;
using _Source.Features.GameRound.Data;
using _Source.Features.Movement.Data;
using _Source.Features.Sensors.Data;
using Zenject;

namespace _Source.Installation
{
    public class DataRepositoryInstaller : Installer<DataRepositoryInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AvatarData>().AsSingle();
            Container.BindInterfacesAndSelfTo<NovatarData>().AsSingle();
            Container.BindInterfacesAndSelfTo<MovementDataRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<RangeSensorDataRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<WanderDataRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameRoundDataRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<StateDelaysRepository>().AsSingle();
        }
    }
}
