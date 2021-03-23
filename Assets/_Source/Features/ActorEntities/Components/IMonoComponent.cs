using _Source.Features.Actors;
using UniRx;

namespace _Source.Features.ActorEntities.Components
{
    public interface IMonoComponent
    {
        void Setup(IActorStateModel actor);
        void StartComponent(CompositeDisposable disposer);
        void StopComponent();
    }
}
