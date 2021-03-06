﻿using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class LeaveScreenNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, AiMovementController, LeaveScreenNode> { }

        private readonly AiMovementController _aiMovementController;
        private readonly ScreenSizeController _screenSizeController;

        private readonly OriginDataComponent _originDataComponent;
        private readonly TransformDataComponent _transformDataComponent;

        public LeaveScreenNode(
            IActorStateModel actorStateModel,
            AiMovementController aiMovementController,
            ScreenSizeController screenSizeController)
        {
            _aiMovementController = aiMovementController;
            _screenSizeController = screenSizeController;

            _originDataComponent = actorStateModel.Get<OriginDataComponent>();
            _transformDataComponent = actorStateModel.Get<TransformDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!IsWithinScreenBounds())
            {
                return BehaviourTreeStatus.Success;
            }

            _aiMovementController.MoveToTarget(_originDataComponent.SpawnPosition.Value);
            return BehaviourTreeStatus.Running;
        }

        private bool IsWithinScreenBounds()
        {
            return !_screenSizeController.IsOutOfScreenBounds(
                _transformDataComponent.Position,
                _transformDataComponent.Size);
        }
    }
}
