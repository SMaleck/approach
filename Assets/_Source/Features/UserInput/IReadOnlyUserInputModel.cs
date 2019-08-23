using UniRx;
using UnityEngine;

namespace _Source.Features.UserInput
{
    public interface IReadOnlyUserInputModel
    {
        IReadOnlyReactiveProperty<Vector2> InputVector { get; }
        bool HasInput { get; }
    }
}
