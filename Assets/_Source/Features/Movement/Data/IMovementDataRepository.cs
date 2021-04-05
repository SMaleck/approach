using _Source.Features.Actors;
using _Source.Features.Actors.Data;

namespace _Source.Features.Movement.Data
{
    public interface IMovementDataRepository
    {
        IMovementData this[EntityType type] { get; }
    }
}