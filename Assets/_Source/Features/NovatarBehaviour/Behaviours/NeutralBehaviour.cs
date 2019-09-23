using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Behaviours
{
    public class NeutralBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, MovementController, NeutralBehaviour> { }

        private readonly MovementController _movementController;
        private readonly ScreenSizeController _screenSizeController;

        private readonly IBehaviourTreeNode _behaviourTree;

        public NeutralBehaviour(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            MovementController movementController,
            ScreenSizeController screenSizeController)
            : base(novatarEntity, novatarStateModel)
        {
            _movementController = movementController;
            _screenSizeController = screenSizeController;

            _behaviourTree = CreateTree();
        }

        public override IBehaviourTreeNode Build()
        {
            return _behaviourTree;
        }

        private IBehaviourTreeNode CreateTree()
        {
            return new BehaviourTreeBuilder()
                .Selector(nameof(NeutralBehaviour))
                    .Sequence("LeavePlayingField")
                        .Condition(nameof(IsWithinScreenBounds), t => IsWithinScreenBounds())
                        .Do(nameof(MoveToSpawnPosition), t => MoveToSpawnPosition())
                        .End()
                    .Sequence("Deactivate")
                        .Condition(nameof(IsWithinScreenBounds), t => !IsWithinScreenBounds())
                        .Do(nameof(DeactivateSelf), t => DeactivateSelf())
                        .End()
                .End()
                .Build();
        }

        private bool IsWithinScreenBounds()
        {
            return !_screenSizeController.IsOutOfScreenBounds(
                NovatarEntity.Position,
                NovatarEntity.Size);
        }

        private BehaviourTreeStatus MoveToSpawnPosition()
        {
            var spawnPosition = NovatarStateModel.SpawnPosition.Value;
            _movementController.MoveToTarget(spawnPosition);
            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus DeactivateSelf()
        {
            NovatarEntity.Deactivate();
            return BehaviourTreeStatus.Success;
        }
    }
}
