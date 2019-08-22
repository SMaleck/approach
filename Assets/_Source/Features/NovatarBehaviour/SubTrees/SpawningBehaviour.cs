using _Source.Entities.Novatar;
using _Source.Features.GameWorld;
using FluentBehaviourTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public class SpawningBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<INovatar, NovatarStateModel, SpawningBehaviour> { }

        private readonly ScreenSizeModel _screenSizeModel;

        private readonly IBehaviourTreeNode _behaviourTree;
        private Vector3 _movementTarget;

        public SpawningBehaviour(
            INovatar novatarEntity,
            NovatarStateModel novatarStateModel,
            ScreenSizeModel screenSizeModel)
            : base(novatarEntity, novatarStateModel)
        {
            _screenSizeModel = screenSizeModel;

            _behaviourTree = CreateTree();

            NovatarStateModel.OnReset
                .Subscribe(_ => Reset())
                .AddTo(Disposer);
        }

        private void Reset()
        {
            var spawnPosition = NovatarStateModel.SpawnPosition.Value;

            // This is correct when spawning at the bottom edge
            var lookRotation = 0;
            var moveDistance = NovatarEntity.Size.y * 2;
            _movementTarget = spawnPosition + new Vector3(0, moveDistance, 0);

            if (spawnPosition.x < -_screenSizeModel.WidthExtendUnits)
            {
                lookRotation = -90;
                _movementTarget = spawnPosition + new Vector3(moveDistance, 0, 0);
            }
            else if (spawnPosition.x > _screenSizeModel.WidthExtendUnits)
            {
                lookRotation = 90;
                _movementTarget = spawnPosition - new Vector3(moveDistance, 0, 0);
            }
            else if (spawnPosition.y > _screenSizeModel.HeightExtendUnits)
            {
                lookRotation = 180;
                _movementTarget = spawnPosition - new Vector3(0, moveDistance, 0);
            }

            NovatarEntity.LookAt(new Vector3(0, 0, lookRotation));
            NovatarEntity.TurnLightsOn();
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
            NovatarEntity.MoveForward();
            return BehaviourTreeStatus.Success;
        }
    }
}
