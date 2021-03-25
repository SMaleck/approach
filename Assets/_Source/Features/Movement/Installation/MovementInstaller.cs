using _Source.Features.Actors;
using Zenject;

namespace _Source.Features.Movement.Installation
{
    public class MovementInstaller : Installer<MovementInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<IActorStateModel, MovementController, MovementController.Factory>();
        }
    }
}
