using UniRx;
using UnityEngine;

namespace _Source.Features.UserInput
{
    public interface IReadOnlyUserInputModel
    {
        IReadOnlyReactiveProperty<float> HorizontalAxis { get; }
        IReadOnlyReactiveProperty<float> VerticalAxis { get; }
        IReadOnlyReactiveProperty<Vector2> InputVector { get; }
        bool HasInput { get; }
    }
}
