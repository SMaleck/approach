using _Source.Features.Actors;
using UniRx;

namespace _Source.Entities.Components
{
    public interface IMonoComponent
    {
        void Setup(IActorStateModel actorStateModel);
        void StartLifeCycle(CompositeDisposable disposer);
    }
}
