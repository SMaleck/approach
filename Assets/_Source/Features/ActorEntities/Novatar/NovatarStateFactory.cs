using _Source.Features.ActorEntities.Novatar.Data;
using _Source.Features.Actors;

namespace _Source.Features.ActorEntities.Novatar
{
    public class NovatarStateFactory : AbstractActorStateFactory
    {
        private readonly NovatarData _data;

        public NovatarStateFactory(NovatarData data)
        {
            _data = data;
        }

        public IActorStateModel Create()
        {
            return ActorStateModelFactory.Create()
                .Attach(BlackBoardDataComponentFactory.Create())
                .Attach(EntityTypeDataComponentFactory.Create(EntityType.NPC))
                .Attach(HealthDataComponentFactory.Create(_data))
                .Attach(DamageDataComponentFactory.Create(_data))
                .Attach(MovementDataComponentFactory.Create(_data))
                .Attach(OriginDataComponentFactory.Create())
                .Attach(LightDataComponentFactory.Create())
                .Attach(RelationshipDataComponentFactory.Create())
                .Attach(TransformDataComponentFactory.Create())
                .Attach(SensorDataComponentFactory.Create());
        }
    }
}
