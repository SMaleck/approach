using System;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using _Source.Services.Random;
using BehaviourTreeSystem;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    // ToDo V1 Implement WanderNode
    public class WanderNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, MovementController, WanderNode> { }

        private readonly IRandomNumberService _randomNumberService;
        private readonly MovementController _movementController;
        private readonly BlackBoardDataComponent _blackBoard;
        private readonly OriginDataComponent _originDataComponent;
        private readonly WanderDataComponent _wanderDataComponent;

        public WanderNode(
            IActorStateModel actorStateModel,
            IRandomNumberService randomNumberService,
            MovementController movementController)
        {
            _randomNumberService = randomNumberService;
            _movementController = movementController;
            _blackBoard = actorStateModel.Get<BlackBoardDataComponent>();
            _originDataComponent = actorStateModel.Get<OriginDataComponent>();
            _wanderDataComponent = actorStateModel.Get<WanderDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            var target = FindWanderTarget();
            _blackBoard.MovementTarget.Store(target);

            _movementController.MoveToTarget(target);

            return BehaviourTreeStatus.Success;
        }

        private Vector2 FindWanderTarget()
        {
            if (!_blackBoard.MovementTarget.HasValue || 
                _movementController.IsTargetReached(_blackBoard.MovementTarget.View()))
            {
                return GetRandomizedTarget();
            }

            return _blackBoard.MovementTarget.View();
        }

        private Vector2 GetRandomizedTarget()
        {
            var x = _randomNumberService.Range(
                _wanderDataComponent.WanderMinDistance,
                _wanderDataComponent.WanderMaxDistance);

            var y = _randomNumberService.Range(
                _wanderDataComponent.WanderMinDistance,
                _wanderDataComponent.WanderMaxDistance);

            var target = new Vector2(x, y);
            return AdjustFirstTarget(target);
        }

        private Vector2 AdjustFirstTarget(Vector2 target)
        {
            // If this actor was moved at least once, we do not adjust the target
            if (_blackBoard.MovementTarget.HasValue)
            {
                return target;
            }

            switch (_originDataComponent.SpawnEdge)
            {
                case ScreenEdge.Top:
                    return new Vector2(target.x, Math.Abs(target.y) * -1);

                case ScreenEdge.Right:
                    return new Vector2(Math.Abs(target.x) * -1, target.y);

                case ScreenEdge.Bottom:
                    return new Vector2(target.x, Math.Abs(target.y));

                case ScreenEdge.Left:
                    return new Vector2(Math.Abs(target.x), target.y);

                default:
                    return target;
            }
        }
    }
}
