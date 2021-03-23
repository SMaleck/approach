using _Source.Features.ActorEntities.Components;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
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

        // ToDo Implement Size correctly, this only works implicitly
        public Vector3 Size => Vector3.one;

        [SerializeField] private Transform _locomotionTarget;
        public Transform LocomotionTarget => _locomotionTarget;

        [SerializeField] private Transform _rotationTarget;
        public Transform RotationTarget => _rotationTarget;

        [SerializeField] private Light _headLight;
        public Light HeadLight => _headLight;

        private SerialDisposable _serialDisposable;
        public CompositeDisposable EntityDisposer { get; private set; }
        public IActorStateModel ActorStateModel { get; private set; }
        public HealthDataComponent HealthDataComponent { get; private set; }

        public IMonoComponent[] Components { get; private set; }
        public ITickableMonoComponent[] TickableComponents { get; private set; }

        // ToDo V2 Needed to get IDamageReceiver etc to other Actors via SensorySystem. Is that the best way? 
        public void Setup(IActorStateModel actorStateModel)
        {
            ActorStateModel = ActorStateModel ?? actorStateModel;
            HealthDataComponent = ActorStateModel.Get<HealthDataComponent>();

            Components = GetComponents<IMonoComponent>();
            Components?.ForEach(e => e.Setup(this));

            TickableComponents = Components?.OfType<ITickableMonoComponent>().ToArray();

            _serialDisposable = new SerialDisposable().AddTo(Disposer);
        }

        public void StartEntity(CompositeDisposable disposer)
        {
            EntityDisposer = disposer;
            _serialDisposable.Disposable = disposer;

            Components?.ForEach(e => e.StartLifeCycle());

            // ToDo V0 This should probably be managed by the Facade
            HealthDataComponent.IsAlive
                .IfFalse()
                .Subscribe(_ => StopEntity())
                .AddTo(disposer);
        }

        public void StopEntity()
        {
            _serialDisposable.Disposable?.Dispose();
            Components?.ForEach(e => e.StopLifeCycle());
        }

        public string ToDebugString()
        {
            return $"{gameObject.name} | POS {Position.ToString()}";
        }

        private void Update()
        {
            if (HealthDataComponent != null &&
                HealthDataComponent.IsAlive.Value)
            {
                TickableComponents?.ForEach(e => e.Tick());
            }
        }
    }
}
