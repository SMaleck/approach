using _Source.Entities;
using _Source.Features.Actors;
using _Source.Features.ActorSensors.Data;
using Zenject;

namespace _Source.Features.ActorSensors.Installation
{
    public class ActorSensorsInstaller : Installer<ActorSensorsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<IMonoEntity, IActorStateModel, SensorySystem, SensorySystem.Factory>().AsSingle();
            Container.BindFactory<IRangeSensorData, IMonoEntity, IMonoEntity, RangeSensor, RangeSensor.Factory>().AsSingle();
        }
    }
}
