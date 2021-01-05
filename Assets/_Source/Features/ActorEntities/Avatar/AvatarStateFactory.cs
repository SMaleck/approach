using _Source.Features.ActorEntities.Avatar.Data;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;

namespace _Source.Features.ActorEntities.Avatar
{
    public class AvatarStateFactory
    {
        private readonly AvatarData _data;
        private readonly ActorStateModel.Factory _actorStateModelFactory;
        private readonly HealthDataComponent.Factory _healthDataComponentFactory;
        private readonly SurvivalDataComponent.Factory _survivalDataComponentFactory;

        public AvatarStateFactory(
            AvatarData data,
            ActorStateModel.Factory actorStateModelFactory,
            HealthDataComponent.Factory healthDataComponentFactory,
            SurvivalDataComponent.Factory survivalDataComponentFactory)
        {
            _data = data;
            _actorStateModelFactory = actorStateModelFactory;
            _healthDataComponentFactory = healthDataComponentFactory;
            _survivalDataComponentFactory = survivalDataComponentFactory;
        }

        public IActorStateModel Create()
        {
            return _actorStateModelFactory.Create()
                .Attach(_healthDataComponentFactory.Create(_data))
                .Attach(_survivalDataComponentFactory.Create());
        }
    }
}
