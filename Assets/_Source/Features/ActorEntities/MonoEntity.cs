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

        private bool _isSetup;
        private IMonoComponent[] _components;
        private ITickableMonoComponent[] _tickableComponents;

        public IActorStateModel Actor { get; private set; }

        // ToDo Implement Size correctly, this only works implicitly
        public Vector3 Size => Vector3.one;

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
            _components?.ForEach(e => e.StartComponent(disposer));
        }

        public void StopEntity()
        {
            _components?.ForEach(e => e.StopComponent());
        }

        public void Tick()
        {
            _tickableComponents?.ForEach(e => e.Tick());
        }
    }
}
