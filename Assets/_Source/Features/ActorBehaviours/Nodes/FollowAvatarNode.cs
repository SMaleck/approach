﻿using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FollowAvatarNode : AbstractNode
    {
        // ToDo V0 Rename to better reflect what it is doing
        public class Factory : PlaceholderFactory<IActorStateModel, FollowAvatarNode> { }

        private readonly IActorStateModel _actorStateModel;
        private readonly BlackBoardDataComponent _blackBoard;
        private readonly SensorDataComponent _sensorDataComponent;
        private readonly TransformDataComponent _transformDataComponent;

        public FollowAvatarNode(IActorStateModel actorStateModel)
        {
            _actorStateModel = actorStateModel;
            _blackBoard = _actorStateModel.Get<BlackBoardDataComponent>();
            _sensorDataComponent = _actorStateModel.Get<SensorDataComponent>();
            _transformDataComponent = _actorStateModel.Get<TransformDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_sensorDataComponent.KnowsAvatar)
            {
                return BehaviourTreeStatus.Failure;
            }

            var avatarTransform = _sensorDataComponent.Avatar.Get<TransformDataComponent>();
            if (!_sensorDataComponent.IsInFollowRange(_transformDataComponent, avatarTransform))
            {
                return BehaviourTreeStatus.Failure;
            }
            // ToDo V0 This should then not move. Works because the following MovementNode just moves to where we are
            if (_sensorDataComponent.IsInTouchRange(_transformDataComponent, avatarTransform))
            {
                _blackBoard.MovementTarget.Store(_transformDataComponent.Position);
                return BehaviourTreeStatus.Success;
            }

            _actorStateModel.ResetIdleTimeouts();
            _blackBoard.MovementTarget.Store(avatarTransform.Position);

            return BehaviourTreeStatus.Success;
        }
    }
}
