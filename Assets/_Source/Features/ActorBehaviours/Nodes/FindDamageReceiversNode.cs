using _Source.Entities.Novatar;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using System.Linq;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FindDamageReceiversNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, FindDamageReceiversNode> { }

        private readonly BlackBoardDataComponent _blackBoard;
        private readonly TransformDataComponent _transformDataComponent;
        private readonly SensorDataComponent _sensorDataComponent;

        public FindDamageReceiversNode(IActorStateModel actorStateModel)
        {
            _blackBoard = actorStateModel.Get<BlackBoardDataComponent>();
            _transformDataComponent = actorStateModel.Get<TransformDataComponent>();
            _sensorDataComponent = actorStateModel.Get<SensorDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            // Prefer damaging Avatar
            if (_sensorDataComponent.KnowsAvatar &&
                IsInTouchRange(_sensorDataComponent.Avatar))
            {
                StoreDamageReceiver(_sensorDataComponent.Avatar);
                return BehaviourTreeStatus.Success;
            }

            // Then try to damage friend
            var friendActor = _sensorDataComponent.KnownEntities
                .FirstOrDefault(e => IsFriend(e) && IsInTouchRange(e));

            if (friendActor != null)
            {
                StoreDamageReceiver(friendActor);
                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }

        private bool IsFriend(IActorStateModel actor)
        {
            return actor.Get<RelationshipDataComponent>().Relationship.Value == EntityState.Friend;
        }

        private bool IsInTouchRange(IActorStateModel actor)
        {
            return _sensorDataComponent.IsInTouchRange(
                _transformDataComponent,
                actor.Get<TransformDataComponent>());
        }

        private void StoreDamageReceiver(IActorStateModel actor)
        {
            var receiver = actor.Get<HealthDataComponent>();
            _blackBoard.DamageReceiver.Store(receiver);
        }

    }
}
