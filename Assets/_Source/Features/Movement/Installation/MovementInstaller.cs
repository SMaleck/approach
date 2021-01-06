using _Source.Entities;
using _Source.Features.Actors;
using Zenject;

namespace _Source.Features.Movement.Installation
{
    public class MovementInstaller : Installer<MovementInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<IActorStateModel, MovementModel, MovementModel.Factory>();
            Container.BindFactory<MovementModel, IMonoEntity, MovementController, MovementController.Factory>();
            Container.BindFactory<IMonoEntity, IMovementModel, MovementComponent, MovementComponent.Factory>();
        }
    }
}
