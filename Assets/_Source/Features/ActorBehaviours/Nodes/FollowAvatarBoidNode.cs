using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Sensors;
using BehaviourTreeSystem;
using System.Linq;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FollowAvatarBoidNode : AbstractNode
    {
        private readonly IActorStateModel _actorStateModel;

        public class Factory : PlaceholderFactory<IActorStateModel, FollowAvatarBoidNode> { }

        private readonly BlackBoardDataComponent _blackBoard;
        private readonly SensorDataComponent _sensorDataComponent;
        private readonly TimeoutDataComponent _timeoutDataComponent;

        public FollowAvatarBoidNode(IActorStateModel actorStateModel)
        {
            _actorStateModel = actorStateModel;

            _blackBoard = _actorStateModel.Get<BlackBoardDataComponent>();
            _sensorDataComponent = _actorStateModel.Get<SensorDataComponent>();
            _timeoutDataComponent = _actorStateModel.Get<TimeoutDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_sensorDataComponent.IsAvatarInRange(SensorType.Visual))
            {
                return BehaviourTreeStatus.Failure;
            }

            if (AnyObstacleInComfortRange())
            {
                return BehaviourTreeStatus.Running;
            }

            _timeoutDataComponent.ResetIdleTimeouts();

            var avatarTransform = _sensorDataComponent.Avatar.Get<TransformDataComponent>();
            _blackBoard.MovementTarget.Store(avatarTransform.Position);

            return BehaviourTreeStatus.Success;
        }


        private bool AnyObstacleInComfortRange()
        {
            var selfDistToAvatar = GetDistanceSquared(_actorStateModel, _sensorDataComponent.Avatar);
            if (selfDistToAvatar <= _sensorDataComponent.ComfortRangeUnitsSquared)
            {
                return true;
            }

            return _sensorDataComponent
                .GetInRangeExceptAvatar(SensorType.Visual)
                .Any(actor =>
                {
                    var distToThis = GetDistanceSquared(_actorStateModel, actor);
                    var distToAvatar = GetDistanceSquared(actor, _sensorDataComponent.Avatar);

                    return distToThis <= _sensorDataComponent.ComfortRangeUnitsSquared &&
                           distToAvatar <= selfDistToAvatar;
                });
        }

        private float GetDistanceSquared(IActorStateModel actorA, IActorStateModel actorB)
        {
            var distance = actorA.Get<TransformDataComponent>().Position -
                           actorB.Get<TransformDataComponent>().Position;

            return distance.sqrMagnitude;
        }
    }
}
