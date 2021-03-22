using _Source.Entities.Novatar;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Sensors;
using BehaviourTreeSystem;
using System.Linq;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FindDamageReceiversNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, FindDamageReceiversNode> { }

        private readonly BlackBoardDataComponent _blackBoard;
        private readonly SensorDataComponent _sensorDataComponent;

        public FindDamageReceiversNode(IActorStateModel actorStateModel)
        {
            _blackBoard = actorStateModel.Get<BlackBoardDataComponent>();
            _sensorDataComponent = actorStateModel.Get<SensorDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            // Prefer damaging Avatar
            if (_sensorDataComponent.IsAvatarInRange(SensorType.Touch))
            {
                StoreDamageReceiver(_sensorDataComponent.Avatar);
                return BehaviourTreeStatus.Success;
            }

            // Then try to damage friend
            var friendActor = _sensorDataComponent
                .GetInRange(SensorType.Touch)
                .FirstOrDefault(IsFriend);

            if (friendActor != null)
            {
                StoreDamageReceiver(friendActor);
                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }

        private bool IsFriend(IActorStateModel actor)
        {
            return actor.Get<RelationshipDataComponent>()?.Relationship.Value == EntityState.Friend;
        }

        private void StoreDamageReceiver(IActorStateModel actor)
        {
            var receiver = actor.Get<HealthDataComponent>();
            _blackBoard.DamageReceiver.Store(receiver);
        }
    }
}