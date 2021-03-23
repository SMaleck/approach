using _Source.Features.Actors;
using UniRx;
using UnityEngine;

namespace _Source.Features.ActorEntities.Components
{
    public class AbstractMonoComponent : MonoBehaviour, IMonoComponent
    {
        protected IActorStateModel Actor;
        protected CompositeDisposable Disposer { get; private set; }

        private bool _isSetup;

        public void Setup(IActorStateModel actor)
        {
            if (_isSetup) return;
            _isSetup = true;

            Actor = actor;

            OnSetup();
        }

        public void StartComponent(CompositeDisposable disposer)
        {
            Disposer = disposer;
            OnStart();
        }

        public void StopComponent()
        {
            Disposer?.Dispose();
            OnEnd();
        }

        protected virtual void OnSetup() { }
        protected virtual void OnStart() { }
        protected virtual void OnEnd() { }
    }
}
