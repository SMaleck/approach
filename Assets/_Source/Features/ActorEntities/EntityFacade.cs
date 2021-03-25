using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using _Source.Features.Movement;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities
{
    // ToDo V2 Create Movement MonoComponent to handle movement, so IMovableEntity is not implemented here
    public class EntityFacade : AbstractDisposable, IMovableEntity
    {
        public class Factory : PlaceholderFactory<IMonoEntity, IActorStateModel, EntityFacade> { }

        public IMonoEntity Entity { get; }
        protected readonly IActorStateModel Actor;
        private readonly IPauseStateModel _pauseStateModel;

        private readonly SerialDisposable _entityLifecycleDisposable;
        protected readonly HealthDataComponent HealthDataComponent;

        public Transform LocomotionTarget => Entity.LocomotionTarget;
        public Transform RotationTarget => Entity.RotationTarget;
        public Vector3 Position => Entity.Position;
        public Quaternion Rotation => Entity.Rotation;

        private bool CanTick => HealthDataComponent.IsAlive.Value &&
                                !_pauseStateModel.IsPaused.Value;

        public EntityFacade(
            IMonoEntity entity,
            IActorStateModel actor,
            IPauseStateModel pauseStateModel)
        {
            Entity = entity;
            Actor = actor;
            _pauseStateModel = pauseStateModel;
            _entityLifecycleDisposable = new SerialDisposable().AddTo(Disposer);

            Entity.Setup(Actor);

            Actor.Get<TransformDataComponent>()
                .SetMonoEntity(Entity);

            HealthDataComponent = Actor.Get<HealthDataComponent>();

            HealthDataComponent.IsAlive
                .Subscribe(OnIsAliveChanged)
                .AddTo(Disposer);

            Observable.EveryUpdate()
                .Where(_ => CanTick)
                .Subscribe(_ => Entity.Tick())
                .AddTo(Disposer);
        }

        private void OnIsAliveChanged(bool isAlive)
        {
            Entity.SetActive(isAlive);

            if (isAlive)
            {
                var disposer = new CompositeDisposable();
                _entityLifecycleDisposable.Disposable = disposer;

                Entity.StartEntity(disposer);
            }
            else
            {
                _entityLifecycleDisposable.Disposable?.Dispose();

                Entity.StopEntity();
            }
        }
    }
}
