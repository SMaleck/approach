using System;

namespace _Source.Features.ActorBehaviours.Nodes
{
    [Obsolete("Nodes should not have state")]
    public interface IResettableNode
    {
        void Reset();
    }
}
