using _Source.Features.ActorBehaviours;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.GameRound;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    public class NovatarFacade : EntityFacade, IEntityPoolItem<IMonoEntity>
    {
        private readonly NovatarBehaviourTree _behaviourTree;

        public new class Factory : PlaceholderFactory<IMonoEntity, IActorStateModel, NovatarBehaviourTree, NovatarFacade> { }

        private readonly OriginDataComponent _originDataComponent;

        public bool IsFree => !HealthDataComponent.IsAlive.Value;

        public NovatarFacade(
            IMonoEntity entity,
            IActorStateModel actor,
            NovatarBehaviourTree behaviourTree, 
            IPauseStateModel pauseStateModel)
            : base(entity, actor, pauseStateModel)
        {
            _behaviourTree = behaviourTree;
            _originDataComponent = Actor.Get<OriginDataComponent>();
            _originDataComponent = Actor.Get<OriginDataComponent>();

            _originDataComponent.SpawnPosition
                .Subscribe(Entity.SetPosition)
                .AddTo(Disposer);
        }

        public void Reset(Vector3 spawnPosition)
        {
            _originDataComponent.SetSpawnPosition(spawnPosition);
            Actor.Reset();
            _behaviourTree.Reset();
        }

        protected override void OnTick()
        {
            base.OnTick();
            _behaviourTree.Tick();
        }
    }
}
