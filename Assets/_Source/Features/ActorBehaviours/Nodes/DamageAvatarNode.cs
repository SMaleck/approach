using _Source.Entities.Avatar;
using _Source.Features.ActorBehaviours.Sensors;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    // ToDo This should just execute an attack, but actual damage delivery should be handled differently
    // Currently this can only damage the avatar and IDamageReceiver is being injected, this makes it impossible to be used the other way around
    public class DamageAvatarNode : AbstractNode, IResettableNode
    {
        public class Factory : PlaceholderFactory<ISensorySystem, IActorStateModel, DamageAvatarNode> { }

        private readonly ISensorySystem _sensorySystem;
        private readonly IDamageReceiver _avatarDamageReceiver;

        private readonly DamageDataComponent _damageDataComponent;
        private bool _hasDamagedAvatar;

        public DamageAvatarNode(
            ISensorySystem sensorySystem,
            IDamageReceiver avatarDamageReceiver,
            IActorStateModel actorStateModel)
        {
            _sensorySystem = sensorySystem;
            _avatarDamageReceiver = avatarDamageReceiver;

            _damageDataComponent = actorStateModel.Get<DamageDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (_hasDamagedAvatar)
            {
                return BehaviourTreeStatus.Success;
            }

            if (_sensorySystem.IsInTouchRange())
            {
                var damage = _damageDataComponent.Damage;
                _avatarDamageReceiver.ReceiveDamage(damage);

                _hasDamagedAvatar = true;

                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }

        public void Reset()
        {
            _hasDamagedAvatar = false;
        }
    }
}
