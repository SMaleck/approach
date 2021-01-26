using _Source.Features.Actors;
using UniRx;
using UnityEngine;

namespace _Source.Entities.Components
{
    // ToDo V0 Disposal structure is not great here
    public class AbstractMonoComponent : MonoBehaviour, IMonoComponent
    {
        protected IActorStateModel ActorStateModel;
        protected CompositeDisposable Disposer;

        public void Setup(IActorStateModel actorStateModel)
        {
            ActorStateModel = ActorStateModel ?? actorStateModel;
            
            OnSetup();
        }

        protected virtual void OnSetup() { }

        public void StartLifeCycle(CompositeDisposable disposer)
        {
            if (Disposer != null && !Disposer.IsDisposed)
            {
                Disposer.Dispose();
            }

            Disposer = disposer;

            OnStart();
        }

        protected virtual void OnStart() { }
    }
}
