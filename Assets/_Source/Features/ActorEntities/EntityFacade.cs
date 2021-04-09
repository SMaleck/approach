using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using _Source.Util;
using System;
using UniRx;
using Zenject;

namespace _Source.Features.ActorEntities
{
    public class EntityFacade : AbstractDisposableFeature
    {
        public class Factory : PlaceholderFactory<IMonoEntity, IActorStateModel, EntityFacade> { }

        [Obsolete("Only public for pooling in NovatarSpawner, use TransformDataComponent instead")]
        public IMonoEntity Entity { get; }

        protected readonly IActorStateModel Actor;
        private readonly IPauseStateModel _pauseStateModel;

        private readonly SerialDisposable _entityLifecycleDisposable;
        protected readonly HealthDataComponent HealthDataComponent;

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
                .DelayFrame(1)
                .Subscribe(OnIsAliveChanged)
                .AddTo(Disposer);

            Observable.EveryUpdate()
                .Where(_ => CanTick)
                .Subscribe(_ => OnTick())
                .AddTo(Disposer);
        }

        protected virtual void OnTick()
        {
            Entity.Tick();
        }

        private void OnIsAliveChanged(bool isAlive)
        {
            if (isAlive)
            {
                var disposer = new CompositeDisposable();
                _entityLifecycleDisposable.Disposable = disposer;

                Entity.SetActive(isAlive);
                Entity.StartEntity(disposer);
            }
            else
            {
                _entityLifecycleDisposable.Disposable?.Dispose();

                Entity.StopEntity();
                Entity.SetActive(isAlive);
            }
        }
    }
}
