using _Source.Features.Actors;

namespace _Source.Features.Movement.Data
{
    public interface IMovementDataRepository
    {
        IMovementData this[EntityType type] { get; }
    }
}