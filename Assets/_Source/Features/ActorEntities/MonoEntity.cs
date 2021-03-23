using _Source.Features.ActorEntities.Components;
using _Source.Features.Actors;
using _Source.Util;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities
{
    public class MonoEntity : AbstractDisposableMonoBehaviour, IMonoEntity
    {
        public class Factory : PlaceholderFactory<UnityEngine.Object, MonoEntity> { }

        [SerializeField] private Transform _locomotionTarget;
        public Transform LocomotionTarget => _locomotionTarget;

        [SerializeField] private Transform _rotationTarget;
        public Transform RotationTarget => _rotationTarget;

        [SerializeField] private Light _headLight;
        public Light HeadLight => _headLight;

        private bool _isSetup;
        private bool _isActive;

        private IMonoComponent[] _components;
        private ITickableMonoComponent[] _tickableComponents;

        public IActorStateModel Actor { get; private set; }

        // ToDo Implement Size correctly, this only works implicitly
        public Vector3 Size => Vector3.one;

        // ToDo V2 Needed to get IDamageReceiver etc to other Actors via SensorySystem. Is that the best way? 
        public void Setup(IActorStateModel actorStateModel)
        {
            if (_isSetup) return;
            _isSetup = true;

            Actor = actorStateModel;

            _components = GetComponents<IMonoComponent>();
            _components?.ForEach(e => e.Setup(Actor));

            _tickableComponents = _components?
                .OfType<ITickableMonoComponent>()
                .ToArray();
        }

        public void StartEntity(CompositeDisposable disposer)
        {
            _isActive = true;
            _components?.ForEach(e => e.StartComponent(disposer));
        }

        public void StopEntity()
        {
            _isActive = false;
            _components?.ForEach(e => e.StopComponent());
        }

        private void Update()
        {
            if (_isActive)
            {
                _tickableComponents?.ForEach(e => e.Tick());
            }
        }
    }
}
