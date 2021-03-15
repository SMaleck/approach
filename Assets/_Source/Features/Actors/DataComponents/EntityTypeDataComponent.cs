using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class EntityTypeDataComponent : AbstractDataComponent
    {
        public class Factory : PlaceholderFactory<EntityType, EntityTypeDataComponent> { }

        public EntityType EntityType { get; }

        public EntityTypeDataComponent(EntityType entityType)
        {
            EntityType = entityType;
        }
    }
}
