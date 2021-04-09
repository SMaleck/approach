using _Source.Entities.Novatar;
using _Source.Features.ActorEntities.Config;
using _Source.Features.Actors.DataComponents;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace _Source.Features.ActorEntities.Components
{
    // ToDo V1 ENEMY: Shake
    // ToDo V1 FRIEND: Bump Scale
    public class StateReactionComponent : AbstractMonoComponent
    {
        [SerializeField] private Transform _scaleParent;

        private RelationshipDataComponent _relationshipDataComponent;
        private SerialDisposable _tweenDisposer;

        protected override void OnSetup()
        {
            _relationshipDataComponent = Actor.Get<RelationshipDataComponent>();
        }

        protected override void OnStart()
        {
            _tweenDisposer = new SerialDisposable().AddTo(Disposer);

            _relationshipDataComponent.Relationship
                .Subscribe(OnRelationshipSwitched)
                .AddTo(Disposer);
        }

        protected override void OnStop()
        {
            _tweenDisposer?.Dispose();
        }

        private void OnRelationshipSwitched(EntityState state)
        {

        }

        private void ToEnemy()
        {

        }

        private void ToFriend()
        {

        }
    }
}
