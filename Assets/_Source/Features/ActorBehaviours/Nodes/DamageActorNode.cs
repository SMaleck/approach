using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class DamageActorNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, DamageActorNode> { }

        private readonly BlackBoardDataComponent _blackBoardDataComponent;
        private readonly DamageDataComponent _damageDataComponent;

        public DamageActorNode(IActorStateModel actorStateModel)
        {
            _blackBoardDataComponent = actorStateModel.Get<BlackBoardDataComponent>();
            _damageDataComponent = actorStateModel.Get<DamageDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (_damageDataComponent.HasDeliveredDamage)
            {
                return BehaviourTreeStatus.Failure;
            }

            if (!_blackBoardDataComponent.DamageReceiver.HasValue)
            {
                return BehaviourTreeStatus.Failure;
            }

            var receiver = _blackBoardDataComponent.DamageReceiver.Consume();
            var damage = _damageDataComponent.Damage;

            receiver.ReceiveDamage(damage);
            _damageDataComponent.IncrementHitCount();

            return BehaviourTreeStatus.Success;
        }
    }
}
