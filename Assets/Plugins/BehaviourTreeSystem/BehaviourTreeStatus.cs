using System;

namespace BehaviourTreeSystem
{
    /// <summary>
    /// The return type when invoking behaviour tree nodes.
    /// </summary>
    public enum BehaviourTreeStatus
    {
        Success,
        Failure,

        [Obsolete("Does not work as expected and should be avoided")]
        Running
    }
}