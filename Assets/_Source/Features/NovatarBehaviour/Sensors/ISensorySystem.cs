using UnityEngine;

namespace _Source.Features.NovatarBehaviour.Sensors
{
    public interface ISensorySystem
    {
        Vector3 GetAvatarPosition();
        bool IsInFollowRange();
        bool IsInTouchRange();
    }
}
