using _Source.Features.Actors;
using UniRx;
using UnityEngine;

namespace _Source.Features.ActorEntities
{
    public interface IMonoEntity
    {
        string Name { get; }
        bool IsActive { get; }

        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 Size { get; }

        IActorStateModel ActorStateModel { get; }
        CompositeDisposable EntityDisposer { get; }

        void Setup(IActorStateModel actorStateModel);
        string ToDebugString();
    }
}
