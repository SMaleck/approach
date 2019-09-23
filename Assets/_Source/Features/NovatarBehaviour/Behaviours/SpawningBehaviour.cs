using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using FluentBehaviourTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Behaviours
{
    public class SpawningBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, MovementController, SpawningBehaviour> { }

        private readonly MovementController _movementController;
        private readonly ScreenSizeModel _screenSizeModel;

        private readonly IBehaviourTreeNode _behaviourTree;
        private bool _hasStarted;
        private Vector3 _movementTarget;

        public SpawningBehaviour(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            MovementController movementController,
            ScreenSizeModel screenSizeModel)
            : base(novatarEntity, novatarStateModel)
        {
            _movementController = movementController;
            _screenSizeModel = screenSizeModel;

            _behaviourTree = CreateTree();

            NovatarStateModel.OnReset
                .Subscribe(_ => Reset())
                .AddTo(Disposer);
        }

        private void Reset()
        {
            _hasStarted = false;
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
                    .Sequence("Start")
                        .Condition(nameof(_hasStarted), t => !_hasStarted)
                        .Do(nameof(StartOpeningSequence), t => StartOpeningSequence())
                        .End()
                    .Sequence("Move")
                        .Condition(nameof(_hasStarted), t => _hasStarted)
                        .Do(nameof(Move), t => Move())
                        .End()
                .End()
                .Build();
        }

        private bool HasReachedTarget()
        {
            return _movementController.IsLastTargetReached();
        }

        private BehaviourTreeStatus SwitchToUnacquainted()
        {
            _movementController.Stop();

            NovatarEntity.SwitchToEntityState(EntityState.Unacquainted);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus StartOpeningSequence()
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

            _movementController.SetEulerAngles(new Vector3(0, 0, lookRotation));
            NovatarEntity.TurnLightsOn();

            _hasStarted = true;

            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus Move()
        {
            if (!_movementController.IsMoving())
            {
                _movementController.MoveToTarget(_movementTarget);
            }

            return HasReachedTarget() 
                ? BehaviourTreeStatus.Success
                : BehaviourTreeStatus.Running;
        }
    }
}
