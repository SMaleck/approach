using UnityEngine;

namespace _Source.Features.ActorSensors
{
    public interface ISensorySystem
    {
        Vector3 GetAvatarPosition();
        bool IsInFollowRange();
        bool IsInInteractionRange();
        bool IsInTouchRange();
    }
}
