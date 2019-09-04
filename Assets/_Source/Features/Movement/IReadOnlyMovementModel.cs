using UniRx;
using UnityEngine;

namespace _Source.Features.Movement
{
    public interface IReadOnlyMovementModel
    {
        bool HasMoveIntention { get; }
        IReadOnlyReactiveProperty<Vector2> MoveIntention { get; }

        bool HasTurnIntention { get; }
        IReadOnlyReactiveProperty<Quaternion> TurnIntention { get; }

        float MoveSpeed { get; }
        float TurnSpeed { get; }
    }
}