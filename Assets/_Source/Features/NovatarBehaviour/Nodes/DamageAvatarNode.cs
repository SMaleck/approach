using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.NovatarBehaviour.Sensors;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class DamageAvatarNode : AbstractNode, IResettableNode
    {
        public class Factory : PlaceholderFactory<ISensorySystem, DamageAvatarNode> { }

        private readonly ISensorySystem _sensorySystem;
        private readonly NovatarConfig _novatarConfig;
        private readonly IDamageReceiver _avatarDamageReceiver;

        private bool _hasDamagedAvatar;

        public DamageAvatarNode(
            ISensorySystem sensorySystem,
            NovatarConfig novatarConfig,
            IDamageReceiver avatarDamageReceiver)
        {
            _sensorySystem = sensorySystem;
            _novatarConfig = novatarConfig;
            _avatarDamageReceiver = avatarDamageReceiver;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (_hasDamagedAvatar)
            {
                return BehaviourTreeStatus.Success;
            }

            if (_sensorySystem.IsInTouchRange())
            {
                var damage = _novatarConfig.TouchDamage;
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
