using _Source.Features.ActorEntities.Novatar.Data;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;

namespace _Source.Features.ActorEntities.Novatar
{
    public class NovatarStateFactory
    {
        private readonly NovatarData _data;
        private readonly ActorStateModel.Factory _actorStateModelFactory;
        private readonly HealthDataComponent.Factory _healthDataComponentFactory;
        private readonly DamageDataComponent.Factory _damageDataComponentFactory;
        private readonly MovementDataComponent.Factory _movementDataComponentFactory;
        private readonly OriginDataComponent.Factory _originDataComponentFactory;
        private readonly RelationshipDataComponent.Factory _relationshipDataComponentFactory;

        public NovatarStateFactory(
            NovatarData data,
            ActorStateModel.Factory actorStateModelFactory,
            HealthDataComponent.Factory healthDataComponentFactory,
            DamageDataComponent.Factory damageDataComponentFactory,
            MovementDataComponent.Factory movementDataComponentFactory,
            OriginDataComponent.Factory originDataComponentFactory,
            RelationshipDataComponent.Factory relationshipDataComponentFactory)
        {
            _data = data;
            _actorStateModelFactory = actorStateModelFactory;
            _healthDataComponentFactory = healthDataComponentFactory;
            _damageDataComponentFactory = damageDataComponentFactory;
            _movementDataComponentFactory = movementDataComponentFactory;
            _originDataComponentFactory = originDataComponentFactory;
            _relationshipDataComponentFactory = relationshipDataComponentFactory;
        }

        public IActorStateModel Create()
        {
            return _actorStateModelFactory.Create()
                .Attach(_healthDataComponentFactory.Create(_data))
                .Attach(_damageDataComponentFactory.Create(_data))
                .Attach(_movementDataComponentFactory.Create(_data))
                .Attach(_originDataComponentFactory.Create())
                .Attach(_relationshipDataComponentFactory.Create());
        }
    }
}
