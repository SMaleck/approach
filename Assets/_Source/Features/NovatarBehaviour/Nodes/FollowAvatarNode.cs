using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour.Sensors;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class FollowAvatarNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<ISensorySystem, MovementController, FollowAvatarNode> { }

        private readonly ISensorySystem _sensorySystem;
        private readonly MovementController _movementController;

        public FollowAvatarNode(
            ISensorySystem sensorySystem,
            MovementController movementController)
        {
            _sensorySystem = sensorySystem;
            _movementController = movementController;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_sensorySystem.IsInFollowRange())
            {
                return BehaviourTreeStatus.Failure;
            }
            if (_sensorySystem.IsInTouchRange())
            {
                return BehaviourTreeStatus.Success;
            }

            _movementController.MoveToTarget(_sensorySystem.GetAvatarPosition());
            return BehaviourTreeStatus.Running;
        }
    }
}
