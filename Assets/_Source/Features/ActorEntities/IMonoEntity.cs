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

        IActorStateModel Actor { get; }

        void Setup(IActorStateModel actorStateModel);
        void StartEntity(CompositeDisposable disposer);
        void StopEntity();
    }
}
