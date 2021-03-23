using _Source.Features.Actors;
using Zenject;

namespace _Source.Features.Movement.Installation
{
    public class MovementInstaller : Installer<MovementInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<IActorStateModel, MovementModel, MovementModel.Factory>();
            Container.BindFactory<MovementModel, IMovableEntity, MovementController, MovementController.Factory>();
            Container.BindFactory<IMovableEntity, IMovementModel, MovementComponent, MovementComponent.Factory>();
        }
    }
}
