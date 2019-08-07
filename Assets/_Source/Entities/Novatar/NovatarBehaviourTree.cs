using _Source.Entities.Avatar;
using _Source.Util;
using FluentBehaviourTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Entities.Novatar
{
    public class NovatarBehaviourTree : AbstractDisposable, IInitializable
    {
        private readonly NovatarEntity _novatar;
        private readonly NovatarConfig _novatarConfig;
        private readonly AvatarEntity _avatar;

        private IBehaviourTreeNode _behaviourTree;

        public class Factory : PlaceholderFactory<NovatarEntity, NovatarBehaviourTree> { }

        public NovatarBehaviourTree(
            NovatarEntity novatar,
            NovatarConfig novatarConfig,
            AvatarEntity avatar)
        {
            _novatar = novatar;
            _novatarConfig = novatarConfig;
            _avatar = avatar;
        }

        public void Initialize()
        {
            _behaviourTree = CreateTree();

            Observable.EveryLateUpdate()
                .Subscribe(_ => _behaviourTree.Tick(new TimeData(Time.deltaTime)))
                .AddTo(Disposer);
        }

        private IBehaviourTreeNode CreateTree()
        {
            return new BehaviourTreeBuilder()
                .Sequence("FollowRangeBased")
                .Condition(nameof(IsInMoveRange), t => IsInMoveRange())
                .Do(nameof(FollowAvatar), t => FollowAvatar())
                .End()
                .Build();
        }

        private bool IsInMoveRange()
        {
            var heading = _avatar.HeadingTo(_novatar);
            var sqrDistance = heading.sqrMagnitude;

            var isInRange = sqrDistance <= _novatar.SqrRange;
            var isTouching = sqrDistance <= _novatar.SqrTargetReachedThreshold;

            return isInRange && !isTouching;
        }

        private BehaviourTreeStatus FollowAvatar()
        {
            _novatar.FollowAvatar();
            return BehaviourTreeStatus.Success;
        }
    }
}
