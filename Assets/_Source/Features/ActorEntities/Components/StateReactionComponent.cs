using _Source.Entities.Novatar;
using _Source.Features.ActorEntities.Config;
using _Source.Features.Actors.DataComponents;
using DG.Tweening;
using Packages.RxUtils;
using UniRx;
using UnityEngine;

namespace _Source.Features.ActorEntities.Components
{
    public class StateReactionComponent : AbstractMonoComponent
    {
        [SerializeField] private Transform _reactionParent;
        [SerializeField] private StateReactionConfig _config;

        private RelationshipDataComponent _relationshipDataComponent;
        private CompositeDisposable _friendTweenDisposer;
        private CompositeDisposable _enemyTweenDisposer;

        protected override void OnSetup()
        {
            _relationshipDataComponent = Actor.Get<RelationshipDataComponent>();
        }

        protected override void OnStart()
        {
            _friendTweenDisposer = new CompositeDisposable().AddTo(Disposer);
            _enemyTweenDisposer = new CompositeDisposable().AddTo(Disposer);

            _relationshipDataComponent.Relationship
                .Skip(1)
                .Subscribe(OnRelationshipSwitched)
                .AddTo(Disposer);
        }

        protected override void OnStop()
        {
            _friendTweenDisposer?.Dispose();
            _enemyTweenDisposer?.Dispose();

            _reactionParent.localScale = Vector3.one;
        }

        private void OnRelationshipSwitched(EntityState state)
        {
            switch (state)
            {
                case EntityState.Enemy:
                    ToEnemy();
                    break;

                case EntityState.Friend:
                    ToFriend();
                    break;

                default:
                    break;
            }
        }

        private void ToEnemy()
        {
            _reactionParent.DOShakePosition(
                    _config.EnemyShakeSeconds,
                    _config.EnemyShakeStrength,
                    _config.EnemyShakeVibration)
                .AddTo(_enemyTweenDisposer);
        }

        private void ToFriend()
        {
            _reactionParent.DOScale(
                    _config.FriendGrowScale,
                    _config.FriendGrowSeconds)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic)
                .AddTo(_friendTweenDisposer);
        }
    }
}
