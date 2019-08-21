using _Source.Entities.Novatar;
using FluentBehaviourTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public class SpawningBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<NovatarEntity, NovatarStateModel, SpawningBehaviour> { }

        private readonly NovatarConfig _novatarConfig;

        private readonly IBehaviourTreeNode _behaviourTree;
        private Vector3 _movementTarget;

        public SpawningBehaviour(
            NovatarEntity novatarEntity,
            NovatarStateModel novatarStateModel,
            NovatarConfig novatarConfig)
            : base(novatarEntity, novatarStateModel)
        {
            _novatarConfig = novatarConfig;

            _behaviourTree = CreateTree();

            NovatarStateModel.OnReset
                .Subscribe(_ => Reset())
                .AddTo(Disposer);
        }

        private void Reset()
        {

        }

        public override IBehaviourTreeNode Build()
        {
            return _behaviourTree;
        }

        private IBehaviourTreeNode CreateTree()
        {
            return new BehaviourTreeBuilder()
                .Selector(nameof(EnemyBehaviour))
                        .Sequence("Switch Relationship")
                            .Condition(nameof(HasReachedTarget), t => HasReachedTarget())
                            .Do(nameof(SwitchToUnacquainted), t => SwitchToUnacquainted())
                            .End()
                        .Sequence("Move")
                            .Do(nameof(Move), t => Move())
                            .End()
                    .End()
                .End()
                .Build();
        }

        private bool HasReachedTarget()
        {
            return NovatarEntity.IsMovementTargetReached(_movementTarget);
        }

        private BehaviourTreeStatus SwitchToUnacquainted()
        {
            NovatarStateModel.SetCurrentRelationshipStatus(RelationshipStatus.Unacquainted);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus Move()
        {
            return BehaviourTreeStatus.Success;
        }
    }
}
