using _Source.Features.Tokens;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class EntityTypeDataComponent : AbstractDataComponent
    {
        public class Factory : PlaceholderFactory<EntityType, EntityTypeDataComponent> { }

        public EntityType EntityType { get; }
        public string Id { get; }

        public EntityTypeDataComponent(
            EntityType entityType,
            IIdGenerator idGenerator)
        {
            EntityType = entityType;
            Id = idGenerator.Generate();
        }
    }
}
