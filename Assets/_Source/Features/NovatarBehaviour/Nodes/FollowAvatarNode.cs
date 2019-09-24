using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour.Sensors;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class FollowAvatarNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<RangeSensor, MovementController, FollowAvatarNode> { }

        private readonly RangeSensor _rangeSensor;
        private readonly MovementController _movementController;

        public FollowAvatarNode(
            RangeSensor rangeSensor,
            MovementController movementController)
        {
            _rangeSensor = rangeSensor;
            _movementController = movementController;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_rangeSensor.IsInFollowRange())
            {
                return BehaviourTreeStatus.Failure;
            }
            if (_rangeSensor.IsInTouchRange())
            {
                return BehaviourTreeStatus.Success;
            }

            _movementController.MoveToTarget(_rangeSensor.GetAvatarPosition());
            return BehaviourTreeStatus.Running;
        }
    }
}
