using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using BehaviourTreeSystem;
using UnityEngine;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class EnterScreenNode : AbstractNode, IResettableNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, MovementController, EnterScreenNode> { }

        private readonly MovementController _movementController;
        private readonly ScreenSizeModel _screenSizeModel;

        private readonly OriginDataComponent _originDataComponent;
        private readonly TransformDataComponent _transformDataComponent;
        private Vector3 _movementTarget;

        public EnterScreenNode(
            IActorStateModel actorStateModel,
            MovementController movementController,
            ScreenSizeModel screenSizeModel)
        {
            _movementController = movementController;
            _screenSizeModel = screenSizeModel;

            _originDataComponent = actorStateModel.Get<OriginDataComponent>();
            _transformDataComponent = actorStateModel.Get<TransformDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (_movementController.IsTargetReached(_movementTarget))
            {
                return BehaviourTreeStatus.Success;
            }

            _movementController.MoveToTarget(_movementTarget);
            return BehaviourTreeStatus.Running;
        }

        public void Reset()
        {
            var spawnPosition = _originDataComponent.SpawnPosition.Value;

            // This is correct when spawning at the bottom edge
            var lookRotation = 0;
            var moveDistance = _transformDataComponent.Size.y * 2;
            _movementTarget = spawnPosition + new Vector3(0, moveDistance, 0);

            if (spawnPosition.x < -_screenSizeModel.WidthExtendUnits)
            {
                lookRotation = -90;
                _movementTarget = spawnPosition + new Vector3(moveDistance, 0, 0);
            }
            else if (spawnPosition.x > _screenSizeModel.WidthExtendUnits)
            {
                lookRotation = 90;
                _movementTarget = spawnPosition - new Vector3(moveDistance, 0, 0);
            }
            else if (spawnPosition.y > _screenSizeModel.HeightExtendUnits)
            {
                lookRotation = 180;
                _movementTarget = spawnPosition - new Vector3(0, moveDistance, 0);
            }

            _movementController.SetEulerAngles(new Vector3(0, 0, lookRotation));
        }
    }
}
