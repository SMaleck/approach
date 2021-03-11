using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ActorSensors;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FollowAvatarNode : AbstractNode
    {
        // ToDo V0 Rename to better reflect what it is doing
        public class Factory : PlaceholderFactory<IActorStateModel, ISensorySystem, FollowAvatarNode> { }

        private readonly IActorStateModel _actorStateModel;
        private readonly ISensorySystem _sensorySystem;
        private readonly BlackBoardDataComponent _blackBoard;

        public FollowAvatarNode(
            IActorStateModel actorStateModel,
            ISensorySystem sensorySystem)
        {
            _actorStateModel = actorStateModel;
            _sensorySystem = sensorySystem;
            _blackBoard = _actorStateModel.Get<BlackBoardDataComponent>();
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
            _blackBoard.MovementTarget.Store(_sensorySystem.GetAvatarPosition());

            return BehaviourTreeStatus.Success;
        }
    }
}
