﻿using UniRx;
using UnityEngine;

namespace _Source.Entities.Novatar
{
    public interface INovatarStateModel
    {
        IReadOnlyReactiveProperty<EntityState> CurrentEntityState { get; }
        IReadOnlyReactiveProperty<bool> IsAlive { get; }
        IReadOnlyReactiveProperty<Vector3> SpawnPosition { get; }        
        IOptimizedObservable<Unit> OnReset { get; }
        IOptimizedObservable<Unit> OnResetIdleTimeouts { get; }
    }
}