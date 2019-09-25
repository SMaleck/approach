using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class LeaveScreenNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, MovementController, LeaveScreenNode> { }

        private readonly INovatar _novatarEntity;
        private readonly INovatarStateModel _novatarStateModel;
        private readonly MovementController _movementController;
        private readonly ScreenSizeController _screenSizeController;

        public LeaveScreenNode(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            MovementController movementController,
            ScreenSizeController screenSizeController)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _movementController = movementController;
            _screenSizeController = screenSizeController;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!IsWithinScreenBounds())
            {
                return BehaviourTreeStatus.Success;
            }

            _movementController.MoveToTarget(_novatarStateModel.SpawnPosition.Value);
            return BehaviourTreeStatus.Running;
        }

        private bool IsWithinScreenBounds()
        {
            return !_screenSizeController.IsOutOfScreenBounds(
                _novatarEntity.Position,
                _novatarEntity.Size);
        }
    }
}
