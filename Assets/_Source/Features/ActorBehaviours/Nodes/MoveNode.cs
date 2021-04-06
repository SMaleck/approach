using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class MoveNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, AiMovementController, MoveNode> { }

        private readonly AiMovementController _aiMovementController;
        private readonly BlackBoardDataComponent _blackBoard;

        public MoveNode(
            IActorStateModel actorStateModel,
            AiMovementController aiMovementController)
        {
            _aiMovementController = aiMovementController;
            _blackBoard = actorStateModel.Get<BlackBoardDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_blackBoard.MovementTarget.HasValue)
            {
                return BehaviourTreeStatus.Failure;
            }

            var target = _blackBoard.MovementTarget.View();
            if (_aiMovementController.IsTargetReached(target))
            {
                _blackBoard.MovementTarget.Consume();
                return BehaviourTreeStatus.Success;
            }

            _aiMovementController.MoveToTarget(target);
            return BehaviourTreeStatus.Running;
        }
    }
}
