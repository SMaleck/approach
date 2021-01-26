using _Source.Entities.Components;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using System.Linq;
using UniRx;
using UnityEngine;

namespace _Source.Entities
{
    public class AbstractMonoEntity : AbstractDisposableMonoBehaviour, IMonoEntity
    {
        // ToDo Implement Size correctly, this only works implicitly
        public Vector3 Size => Vector3.one;

        [SerializeField] private Transform _locomotionTarget;
        public Transform LocomotionTarget => _locomotionTarget;

        [SerializeField] private Transform _rotationTarget;
        public Transform RotationTarget => _rotationTarget;

        private SerialDisposable _serialDisposable;
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
            Components?.ForEach(e => e.Setup(ActorStateModel));

            TickableComponents = Components?.OfType<ITickableMonoComponent>().ToArray();

            _serialDisposable = new SerialDisposable().AddTo(Disposer);
        }

        public void StartEntity(CompositeDisposable disposer)
        {
            _serialDisposable.Disposable = disposer;
            Components?.ForEach(e => e.StartLifeCycle(disposer));

            // ToDo V0 This should probably be managed by the Facade
            HealthDataComponent.IsAlive
                .IfFalse()
                .Subscribe(_ => _serialDisposable.Disposable?.Dispose())
                .AddTo(disposer);
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
