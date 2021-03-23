using _Source.Features.Actors;
using UniRx;
using UnityEngine;

namespace _Source.Features.ActorEntities.Components
{
    public class AbstractMonoComponent : MonoBehaviour, IMonoComponent
    {
        private readonly CompositeDisposable _disposer = new CompositeDisposable();

        protected IMonoEntity Entity;
        protected IActorStateModel ActorStateModel => Entity.ActorStateModel;

        // This gets disposed & reset on each lifecycle, so this Component is being reset with it
        protected CompositeDisposable Disposer => Entity.EntityDisposer;

        public void Setup(IMonoEntity entity)
        {
            Entity = Entity ?? entity;

            OnSetup();
        }

        public void StartLifeCycle()
        {
            OnStart();
        }

        public void StopLifeCycle()
        {
            OnEnd();
        }

        protected virtual void OnSetup() { }
        protected virtual void OnStart() { }
        protected virtual void OnEnd() { }
    }
}
