using _Source.Entities.ActorEntities.Novatar.Data;
using _Source.Entities.Actors;
using _Source.Entities.Actors.DataComponents;
using _Source.Util;
using Zenject;

namespace _Source.Entities.ActorEntities.Novatar
{
    public class NovatarSpawner : AbstractDisposable, IInitializable
    {
        private readonly NovatarData _data;
        private readonly ActorStateModel.Factory _actorStateModelFactory;
        private readonly HealthDataComponent.Factory _healthDataComponentFactory;
        private readonly OriginDataComponent.Factory _originDataComponentFactory;
        private readonly RelationshipDataComponent.Factory _relationshipDataComponentFactory;

        public NovatarSpawner(
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

        public void Initialize()
        {
        }

        private void Spawn()
        {
            var actorStateModel = _actorStateModelFactory.Create()
                .Attach(_healthDataComponentFactory.Create(_data))
                .Attach(_originDataComponentFactory.Create())
                .Attach(_relationshipDataComponentFactory.Create());
        }
    }
}
