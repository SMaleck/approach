using _Source.Entities.Actors;
using _Source.Entities.Actors.DataComponents;
using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class LeaveScreenNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<INovatar, IActorStateModel, MovementController, LeaveScreenNode> { }

        private readonly INovatar _novatarEntity;
        private readonly MovementController _movementController;
        private readonly ScreenSizeController _screenSizeController;

        private readonly OriginDataComponent _originDataComponent;

        public LeaveScreenNode(
            INovatar novatarEntity,
            IActorStateModel actorStateModel,
            MovementController movementController,
            ScreenSizeController screenSizeController)
        {
            _novatarEntity = novatarEntity;
            _movementController = movementController;
            _screenSizeController = screenSizeController;

            _originDataComponent = actorStateModel.Get<OriginDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!IsWithinScreenBounds())
            {
                return BehaviourTreeStatus.Success;
            }

            _movementController.MoveToTarget(_originDataComponent.SpawnPosition.Value);
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
