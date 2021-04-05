using _Source.Entities.Novatar;

namespace _Source.Features.Actors.DataComponents
{
    public interface IEntityStateSensitiveDataComponent : IDataComponent
    {
        void OnRelationshipChanged(EntityState entityState);
    }
}
