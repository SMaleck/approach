using UniRx;
using UnityEngine;

namespace _Source.Entities.Novatar
{
    public interface INovatarStateModel
    {
        IReadOnlyReactiveProperty<EntityState> CurrentEntityState { get; }
        IReadOnlyReactiveProperty<bool> IsAlive { get; }
        IReadOnlyReactiveProperty<float> CurrentDistanceToAvatar { get; }
        IReadOnlyReactiveProperty<Vector3> SpawnPosition { get; }        
        IOptimizedObservable<Unit> OnReset { get; }
    }
}