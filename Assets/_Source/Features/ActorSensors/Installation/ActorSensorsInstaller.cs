using _Source.Features.Actors;
using _Source.Features.ActorSensors.Data;
using Zenject;

namespace _Source.Features.ActorSensors.Installation
{
    public class ActorSensorsInstaller : Installer<ActorSensorsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<IActorStateModel, IActorStateModel, IRangeSensorData, SensorySystem, SensorySystem.Factory>().AsSingle();
            Container.BindFactory<IActorStateModel, IActorStateModel, IRangeSensorData, RangeSensor, RangeSensor.Factory>().AsSingle();
        }
    }
}
