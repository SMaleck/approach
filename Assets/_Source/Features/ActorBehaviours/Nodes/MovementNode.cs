using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class MovementNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, MovementController, MovementNode> { }

        private readonly MovementController _movementController;
        private readonly BlackBoardDataComponent _blackBoard;

        public MovementNode(
            IActorStateModel actorStateModel,
            MovementController movementController)
        {
            _movementController = movementController;
            _blackBoard = actorStateModel.Get<BlackBoardDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            // ToDo V0 Should this fail instead?
            if (!_blackBoard.MovementTarget.HasValue)
            {
                return BehaviourTreeStatus.Success;
            }

            var target = _blackBoard.MovementTarget.Consume();
            _movementController.MoveToTarget(target);

            return BehaviourTreeStatus.Success;
        }
    }
}
