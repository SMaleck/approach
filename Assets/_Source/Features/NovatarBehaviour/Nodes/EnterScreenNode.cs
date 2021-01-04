using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.ScreenSize;
using BehaviourTreeSystem;
using UnityEngine;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class EnterScreenNode : AbstractNode, IResettableNode
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, MovementController, EnterScreenNode> { }

        private readonly INovatar _novatarEntity;
        private readonly INovatarStateModel _novatarStateModel;
        private readonly MovementController _movementController;
        private readonly ScreenSizeModel _screenSizeModel;

        private Vector3 _movementTarget;

        public EnterScreenNode(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            MovementController movementController,
            ScreenSizeModel screenSizeModel)
        {
            _novatarEntity = novatarEntity;
            _novatarStateModel = novatarStateModel;
            _movementController = movementController;
            _screenSizeModel = screenSizeModel;
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
            var spawnPosition = _novatarStateModel.SpawnPosition.Value;

            // This is correct when spawning at the bottom edge
            var lookRotation = 0;
            var moveDistance = _novatarEntity.Size.y * 2;
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
