using _Source.Entities.Novatar;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Novatar
{
    // ToDo V0 Get IMovableEntity to not be implemented on this
    public class NovatarFacade : AbstractDisposable, IEntityPoolItem<IMonoEntity>, IMovableEntity
    {
        public class Factory : PlaceholderFactory<MonoEntity, IActorStateModel, NovatarFacade> { }

        private readonly MonoEntity _entity;

        private readonly IActorStateModel _actorStateModel;
        private readonly HealthDataComponent _healthDataComponent;
        private readonly OriginDataComponent _originDataComponent;
        private readonly RelationshipDataComponent _relationshipDataComponent;

        public IMonoEntity Entity => _entity;

        public Transform LocomotionTarget => _entity.LocomotionTarget;
        public Transform RotationTarget => _entity.RotationTarget;

        // ToDo V0 Most properties below should probably go into another data component
        public string Name => _entity.Name;
        public bool IsActive => _entity.IsActive;
        public Vector3 Position => _entity.Position;
        public Quaternion Rotation => _entity.Rotation;

        public bool IsFree { get; private set; }

        public NovatarFacade(
            MonoEntity entity,
            IActorStateModel actorStateModel)
        {
            _entity = entity;
            _actorStateModel = actorStateModel;

            _entity.Setup(_actorStateModel);

            _healthDataComponent = _actorStateModel.Get<HealthDataComponent>();
            _originDataComponent = _actorStateModel.Get<OriginDataComponent>();
            _relationshipDataComponent = _actorStateModel.Get<RelationshipDataComponent>();

            _actorStateModel.Get<TransformDataComponent>()
                .SetMonoEntity(_entity);

            _healthDataComponent.IsAlive
                .Subscribe(OnIsAliveChanged)
                .AddTo(Disposer);

            _originDataComponent.SpawnPosition
                .Subscribe(_entity.SetPosition)
                .AddTo(Disposer);
        }

        public void Reset(Vector3 spawnPosition)
        {
            _relationshipDataComponent.SetRelationship(EntityState.Spawning);
            _originDataComponent.SetSpawnPosition(spawnPosition);

            _actorStateModel.Reset();
        }

        private void OnIsAliveChanged(bool isAlive)
        {
            _entity.SetActive(isAlive);
            IsFree = !isAlive;

            if (isAlive)
            {
                _entity.StartEntity(new CompositeDisposable());
            }
            else
            {
                _entity.StopEntity();
            }
        }
    }
}
