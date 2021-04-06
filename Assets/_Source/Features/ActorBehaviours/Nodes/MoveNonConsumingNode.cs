using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class MoveNonConsumingNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, MovementController, MoveNonConsumingNode> { }

        private readonly MovementController _movementController;
        private readonly BlackBoardDataComponent _blackBoard;

        public MoveNonConsumingNode(
            IActorStateModel actorStateModel,
            MovementController movementController)
        {
            _movementController = movementController;
            _blackBoard = actorStateModel.Get<BlackBoardDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_blackBoard.MovementTarget.HasValue)
            {
                return BehaviourTreeStatus.Failure;
            }
            
            var target = _blackBoard.MovementTarget.View();
            if (!_movementController.IsTargetReached(target))
            {
                _blackBoard.MovementTarget.Consume();
                return BehaviourTreeStatus.Success;
            }

            _movementController.MoveToTarget(target);

            return BehaviourTreeStatus.Running;
        }
    }
}
