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
        private readonly OriginDataComponent.Factory _originDataComponentFactory;
        private readonly RelationshipDataComponent.Factory _relationshipDataComponentFactory;

        public NovatarStateFactory(
            NovatarData data,
            ActorStateModel.Factory actorStateModelFactory,
            HealthDataComponent.Factory healthDataComponentFactory,
            OriginDataComponent.Factory originDataComponentFactory,
            RelationshipDataComponent.Factory relationshipDataComponentFactory)
        {
            _data = data;
            _actorStateModelFactory = actorStateModelFactory;
            _healthDataComponentFactory = healthDataComponentFactory;
            _originDataComponentFactory = originDataComponentFactory;
            _relationshipDataComponentFactory = relationshipDataComponentFactory;
        }

        public IActorStateModel Create()
        {
            return _actorStateModelFactory.Create()
                .Attach(_healthDataComponentFactory.Create(_data))
                .Attach(_originDataComponentFactory.Create())
                .Attach(_relationshipDataComponentFactory.Create());
        }
    }
}
