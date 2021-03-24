using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using _Source.Util;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorEntities.Avatar
{
    // ToDo V0 Get IMovableEntity to not be implemented on this
    // ToDo V0 No/AvatarFacade should be reduced in scope to a container
    // ToDo V0 Create Movement MonoComponent to handle movement
    public class AvatarFacade : AbstractDisposableFeature, IMovableEntity
    {
        public class Factory : PlaceholderFactory<IMonoEntity, IActorStateModel, AvatarFacade> { }

        private readonly IMonoEntity _entity;

        public Vector3 Position => _entity.Position;
        public Quaternion Rotation => _entity.Rotation;

        public Transform LocomotionTarget => _entity.LocomotionTarget;
        public Transform RotationTarget => _entity.RotationTarget;

        public string Name => _entity.Name;
        

        public AvatarFacade(
            IMonoEntity entity,
            IActorStateModel actorStateModel)
        {
            _entity = entity;

            entity.Setup(actorStateModel);

            actorStateModel.Get<TransformDataComponent>()
                .SetMonoEntity(_entity);

            entity.StartEntity(this.Disposer);

            var healthDataComponent = actorStateModel.Get<HealthDataComponent>();
            healthDataComponent.IsAlive
                .IfFalse()
                .Subscribe(_ => entity.StopEntity())
                .AddTo(Disposer);
        }
    }
}
