using _Source.Features.Actors;
using _Source.Features.ActorSensors;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FollowAvatarNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, ISensorySystem, MovementController, FollowAvatarNode> { }

        private readonly IActorStateModel _actorStateModel;
        private readonly ISensorySystem _sensorySystem;
        private readonly MovementController _movementController;

        public FollowAvatarNode(
            IActorStateModel actorStateModel,
            ISensorySystem sensorySystem,
            MovementController movementController)
        {
            _actorStateModel = actorStateModel;
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

            _actorStateModel.ResetIdleTimeouts();
            _movementController.MoveToTarget(_sensorySystem.GetAvatarPosition());

            return BehaviourTreeStatus.Running;
        }
    }
}
