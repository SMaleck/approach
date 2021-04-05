using _Source.Util;

namespace _Source.Features.Actors.DataSystems
{
    public abstract class AbstractDataSystem : AbstractDisposable, IDataSystem
    {
        protected AbstractDataSystem(IActorStateModel actor)
        {
            actor.AttachSystem(this);
        }
    }
}
