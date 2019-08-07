using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Util;
using FluentBehaviourTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour
{
    public class NovatarBehaviourTree : AbstractDisposable, IInitializable
    {
        public class Factory : PlaceholderFactory<NovatarEntity, NovatarStateModel, NovatarBehaviourTree> { }

        private readonly NovatarEntity _novatar;
        private readonly NovatarStateModel _novatarStateModel;
        private readonly NovatarConfig _novatarConfig;
        private readonly AvatarEntity _avatar;

        private IBehaviourTreeNode _behaviourTree;

        public NovatarBehaviourTree(
            NovatarEntity novatar,
            NovatarStateModel novatarStateModel,
            NovatarConfig novatarConfig,
            AvatarEntity avatar)
        {
            _novatar = novatar;
            _novatarStateModel = novatarStateModel;
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
                .Parallel("Tree", 20, 20)
                    .Do(nameof(CalculateDistanceToAvatar), t => CalculateDistanceToAvatar())
                    .Selector("RangeBasedSelection")
                        .Sequence("FollowAvatar")
                            .Condition(nameof(IsInFollowRange), t => IsInFollowRange())
                            .Do(nameof(FollowAvatar), t => FollowAvatar())
                            .End()
                        .Sequence("TouchAvatar")
                            .Condition(nameof(IsInTouchRange), t => IsInTouchRange())
                            .Do(nameof(EvaluateRelationship), t => EvaluateRelationship())
                            .End()
                    .End()
                .End()
                .Build();
        }

        private BehaviourTreeStatus CalculateDistanceToAvatar()
        {
            var sqrDistance = _novatar.GetSquaredDistanceTo(_avatar);
            _novatarStateModel.SetCurrentDistanceToAvatar(sqrDistance);

            return BehaviourTreeStatus.Success;
        }

        private bool IsInFollowRange()
        {
            var isInRange = _novatarStateModel.CurrentDistanceToAvatar.Value <= _novatar.SqrRange;
            return isInRange && !IsInTouchRange();
        }

        private bool IsInTouchRange()
        {
            return _novatarStateModel.CurrentDistanceToAvatar.Value <= _novatar.SqrTargetReachedThreshold;
        }

        private BehaviourTreeStatus FollowAvatar()
        {
            _novatar.FollowAvatar();

            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus EvaluateRelationship()
        {
            App.Logger.Log("EVALUATING");

            return BehaviourTreeStatus.Success;
        }
    }
}
