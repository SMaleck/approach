using _Source.Features.ActorEntities.Avatar.Data;
using _Source.Features.Actors;

namespace _Source.Features.ActorEntities.Avatar
{
    public class AvatarStateFactory : AbstractActorStateFactory
    {
        private readonly AvatarData _data;

        public AvatarStateFactory(AvatarData data)
        {
            _data = data;
        }

        public IActorStateModel Create()
        {
            return ActorStateModelFactory.Create()
                .Attach(HealthDataComponentFactory.Create(_data))
                .Attach(MovementDataComponentFactory.Create(_data))
                .Attach(SurvivalDataComponentFactory.Create())
                .Attach(TransformDataComponentFactory.Create())
                .Attach(SensorDataComponentFactory.Create());
        }
    }
}
