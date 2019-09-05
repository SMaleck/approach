using UniRx;
using UnityEngine;

namespace _Source.Features.UserInput
{
    public interface IVirtualJoystickModel
    {
        IReadOnlyReactiveProperty<bool> IsPointerDown { get; }

        IReadOnlyReactiveProperty<Vector2> StartPointerPosition { get; }
        IReadOnlyReactiveProperty<Vector2> CurrentPointerPosition { get; }    
    }
}
