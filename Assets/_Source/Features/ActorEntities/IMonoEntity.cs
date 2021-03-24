using _Source.Features.Actors;
using _Source.Util;
using UniRx;
using UnityEngine;

namespace _Source.Features.ActorEntities
{
    public interface IMonoEntity : IMonoBehaviour
    {
        Vector3 Size { get; }

        Transform LocomotionTarget { get; }
        Transform RotationTarget { get; }

        IActorStateModel Actor { get; }

        void Setup(IActorStateModel actorStateModel);
        void StartEntity(CompositeDisposable disposer);
        void StopEntity();
    }
}
