using _Source.Features.ActorEntities.Avatar.Data;
using _Source.Features.ActorEntities.Novatar.Data;
using _Source.Features.Actors.DataComponents;
using Zenject;

namespace _Source.Features.Actors.Creation
{
    public class ActorFactory : IAvatarActorFactory, INovatarActorFactory
    {
        [Inject] private readonly ActorStateModel.Factory _actorStateModelFactory;
        [Inject] private readonly BlackBoardDataComponent.Factory _blackBoardDataComponentFactory;
        [Inject] private readonly EntityTypeDataComponent.Factory _entityTypeDataComponentFactory;
        [Inject] private readonly HealthDataComponent.Factory _healthDataComponentFactory;
        [Inject] private readonly DamageDataComponent.Factory _damageDataComponentFactory;
        [Inject] private readonly MovementDataComponent.Factory _movementDataComponentFactory;
        [Inject] private readonly OriginDataComponent.Factory _originDataComponentFactory;
        [Inject] private readonly RelationshipDataComponent.Factory _relationshipDataComponentFactory;
        [Inject] private readonly LightDataComponent.Factory _lightDataComponentFactory;
        [Inject] private readonly TransformDataComponent.Factory _transformDataComponentFactory;
        [Inject] private readonly SensorDataComponent.Factory _sensorDataComponentFactory;
        [Inject] private readonly TimeoutDataComponent.Factory _timeoutDataComponentFactory;
        [Inject] private readonly WanderDataComponent.Factory _wanderDataComponentFactory;

        private readonly AvatarData _avatarData;
        private readonly NovatarData _novatarData;

        public ActorFactory(
            AvatarData avatarData,
            NovatarData novatarData)
        {
            _avatarData = avatarData;
            _novatarData = novatarData;
        }

        public IActorStateModel CreateAvatar()
        {
            return _actorStateModelFactory.Create()
                .Attach(_entityTypeDataComponentFactory.Create(EntityType.Avatar))
                .Attach(_healthDataComponentFactory.Create(_avatarData))
                .Attach(_movementDataComponentFactory.Create(_avatarData))
                .Attach(_transformDataComponentFactory.Create())
                .Attach(_sensorDataComponentFactory.Create());
        }

        public IActorStateModel CreateNovatar()
        {
            return _actorStateModelFactory.Create()
                .Attach(_blackBoardDataComponentFactory.Create())
                .Attach(_entityTypeDataComponentFactory.Create(EntityType.NPC))
                .Attach(_healthDataComponentFactory.Create(_novatarData))
                .Attach(_damageDataComponentFactory.Create(_novatarData))
                .Attach(_movementDataComponentFactory.Create(_novatarData))
                .Attach(_originDataComponentFactory.Create())
                .Attach(_lightDataComponentFactory.Create())
                .Attach(_relationshipDataComponentFactory.Create())
                .Attach(_transformDataComponentFactory.Create())
                .Attach(_sensorDataComponentFactory.Create())
                .Attach(_timeoutDataComponentFactory.Create())
                .Attach(_wanderDataComponentFactory.Create(_novatarData));
        }
    }
}
