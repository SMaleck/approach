using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ScreenSize;
using _Source.Services.Random;
using BehaviourTreeSystem;
using System;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class WanderNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, WanderNode> { }

        private readonly IRandomNumberService _randomNumberService;
        private readonly ScreenSizeModel _screenSizeModel;
        private readonly BlackBoardDataComponent _blackBoard;
        private readonly OriginDataComponent _originDataComponent;
        private readonly WanderDataComponent _wanderDataComponent;
        private readonly TransformDataComponent _transformDataComponent;

        private readonly int[] _directions = { -1, 1 };

        public WanderNode(
            IActorStateModel actorStateModel,
            IRandomNumberService randomNumberService,
            ScreenSizeModel screenSizeModel)
        {
            _randomNumberService = randomNumberService;
            _screenSizeModel = screenSizeModel;
            _blackBoard = actorStateModel.Get<BlackBoardDataComponent>();
            _originDataComponent = actorStateModel.Get<OriginDataComponent>();
            _wanderDataComponent = actorStateModel.Get<WanderDataComponent>();
            _transformDataComponent = actorStateModel.Get<TransformDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (_blackBoard.MovementTarget.HasValue)
            {
                return BehaviourTreeStatus.Success;
            }

            var target = FindWanderTarget();

            _blackBoard.MovementTarget.Store(target);
            _blackBoard.HasWanderedOnce = true;

            return BehaviourTreeStatus.Success;
        }

        private Vector2 FindWanderTarget()
        {
            if (!_blackBoard.MovementTarget.HasValue)
            {
                return GetRandomizedTarget();
            }

            return _blackBoard.MovementTarget.View();
        }

        private Vector2 GetRandomizedTarget()
        {
            var xDir = _randomNumberService.FromSet(_directions);
            var yDir = _randomNumberService.FromSet(_directions);
            var distance = _randomNumberService.Range(
                _wanderDataComponent.WanderMinDistance,
                _wanderDataComponent.WanderMaxDistance);

            var relativeTarget = new Vector3(xDir, yDir, 0) * distance;
            relativeTarget = AdjustFirstTarget(relativeTarget);

            var target = _transformDataComponent.Position + relativeTarget;
            target = _screenSizeModel.GetClampedPosition(target);

            return target;
        }

        private Vector2 AdjustFirstTarget(Vector2 target)
        {
            // If this actor was moved at least once, we do not adjust the target
            if (_blackBoard.HasWanderedOnce)
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
