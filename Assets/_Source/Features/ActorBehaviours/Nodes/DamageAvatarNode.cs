using _Source.Entities.Avatar;
using _Source.Features.ActorEntities.Avatar;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ActorSensors;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    // ToDo V0 This should just execute an attack, but actual damage delivery should be handled differently
    // Currently this can only damage the avatar and IDamageReceiver is being injected, this makes it impossible to be used the other way around
    public class DamageAvatarNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<ISensorySystem, IActorStateModel, DamageAvatarNode> { }

        private readonly ISensorySystem _sensorySystem;
        private readonly IAvatarLocator _avatarLocator;
        private IDamageReceiver DamageReceiver => _avatarLocator.AvatarDamageReceiver;

        private readonly DamageDataComponent _damageDataComponent;

        public DamageAvatarNode(
            ISensorySystem sensorySystem,
            IActorStateModel actorStateModel,
            IAvatarLocator avatarLocator)
        {
            _sensorySystem = sensorySystem;
            _avatarLocator = avatarLocator;

            _damageDataComponent = actorStateModel.Get<DamageDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (_damageDataComponent.HasDeliveredDamage)
            {
                return BehaviourTreeStatus.Success;
            }

            if (_sensorySystem.IsInTouchRange())
            {
                var damage = _damageDataComponent.Damage;
                DamageReceiver.ReceiveDamage(damage);
                _damageDataComponent.IncrementHitCount();

                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }
    }
}
