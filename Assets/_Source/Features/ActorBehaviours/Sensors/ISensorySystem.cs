using UnityEngine;

namespace _Source.Features.ActorBehaviours.Sensors
{
    public interface ISensorySystem
    {
        Vector3 GetAvatarPosition();
        bool IsInFollowRange();
        bool IsInInteractionRange();
        bool IsInTouchRange();
    }
}
