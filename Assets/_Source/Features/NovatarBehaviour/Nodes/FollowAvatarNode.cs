using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour.Sensors;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class FollowAvatarNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<INovatar , ISensorySystem, MovementController, FollowAvatarNode> { }

        private readonly INovatar _novatarEntity;
        private readonly ISensorySystem _sensorySystem;
        private readonly MovementController _movementController;

        public FollowAvatarNode(
            INovatar novatarEntity,
            ISensorySystem sensorySystem,
            MovementController movementController)
        {
            _novatarEntity = novatarEntity;
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

            _novatarEntity.ResetIdleTimeouts();
            _movementController.MoveToTarget(_sensorySystem.GetAvatarPosition());

            return BehaviourTreeStatus.Running;
        }
    }
}
