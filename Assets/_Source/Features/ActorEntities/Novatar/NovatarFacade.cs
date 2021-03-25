using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    public class NovatarFacade : EntityFacade, IEntityPoolItem<IMonoEntity>
    {
        public new class Factory : PlaceholderFactory<IMonoEntity, IActorStateModel, NovatarFacade> { }

        private readonly OriginDataComponent _originDataComponent;

        public bool IsFree => !HealthDataComponent.IsAlive.Value;

        public NovatarFacade(
            IMonoEntity entity,
            IActorStateModel actor)
            : base(entity, actor)
        {
            _originDataComponent = Actor.Get<OriginDataComponent>();

            _originDataComponent.SpawnPosition
                .Subscribe(Entity.SetPosition)
                .AddTo(Disposer);
        }

        public void Reset(Vector3 spawnPosition)
        {
            _originDataComponent.SetSpawnPosition(spawnPosition);
            Actor.Reset();
        }
    }
}
