using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Sensors;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FollowAvatarBoidNode : AbstractNode
    {
        // ToDo V1 Implement FollowAvatarBoidNode
        public class Factory : PlaceholderFactory<IActorStateModel, FollowAvatarBoidNode> { }

        private readonly IActorStateModel _actorStateModel;
        private readonly BlackBoardDataComponent _blackBoard;
        private readonly SensorDataComponent _sensorDataComponent;
        private readonly TransformDataComponent _transformDataComponent;
        private readonly TimeoutDataComponent _timeoutDataComponent;

        public FollowAvatarBoidNode(IActorStateModel actorStateModel)
        {
            _actorStateModel = actorStateModel;
            _blackBoard = _actorStateModel.Get<BlackBoardDataComponent>();
            _sensorDataComponent = _actorStateModel.Get<SensorDataComponent>();
            _transformDataComponent = _actorStateModel.Get<TransformDataComponent>();
            _timeoutDataComponent = _actorStateModel.Get<TimeoutDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_sensorDataComponent.IsAvatarInRange(SensorType.Visual))
            {
                return BehaviourTreeStatus.Failure;
            }

            // ToDo V0 This should then not move. Works because the following MovementNode just moves to where we are
            if (_sensorDataComponent.IsAvatarInRange(SensorType.Touch))
            {
                _blackBoard.MovementTarget.Store(_transformDataComponent.Position);
                return BehaviourTreeStatus.Success;
            }

            _timeoutDataComponent.ResetIdleTimeouts();

            var avatarTransform = _sensorDataComponent.Avatar.Get<TransformDataComponent>();
            _blackBoard.MovementTarget.Store(avatarTransform.Position);

            return BehaviourTreeStatus.Success;
        }
    }
}
