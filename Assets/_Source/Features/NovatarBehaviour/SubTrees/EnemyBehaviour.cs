using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.AvatarState;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public class EnemyBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<NovatarEntity, NovatarStateModel, EnemyBehaviour> { }

        private readonly NeutralBehaviour.Factory _neutralBehaviourFactory;
        private readonly NovatarConfig _novatarConfig;
        private readonly IDamageReceiver _avatarDamageReceiver;
        private readonly AvatarEntity _avatar;

        private readonly IBehaviourTreeNode _behaviourTree;

        private bool _hasDamagedAvatar = false;

        public EnemyBehaviour(
            NovatarEntity novatarEntity,
            NovatarStateModel novatarStateModel,
            NeutralBehaviour.Factory neutralBehaviourFactory,
            NovatarConfig novatarConfig,
            IDamageReceiver avatarDamageReceiver)
            : base(novatarEntity, novatarStateModel)
        {
            _neutralBehaviourFactory = neutralBehaviourFactory;
            _novatarConfig = novatarConfig;
            _avatarDamageReceiver = avatarDamageReceiver;

            _behaviourTree = CreateTree();
        }

        public override IBehaviourTreeNode Build()
        {
            return _behaviourTree;
        }

        private IBehaviourTreeNode CreateTree()
        {
            var neutralBehaviour = _neutralBehaviourFactory
                .Create(
                    NovatarEntity,
                    NovatarStateModel)
                .Build();

            return new BehaviourTreeBuilder()
                .Selector(nameof(EnemyBehaviour))
                    .Sequence("Neutral")
                        .Condition("HasDamagedPlayer", t => _hasDamagedAvatar)
                        .Splice(neutralBehaviour)
                        .End()
                    .Sequence("DamageAvatar")
                        .Condition(nameof(IsInTouchRange), t => IsInTouchRange())
                        .Do(nameof(DamageAvatar), t => DamageAvatar())
                        .End()
                .End()
                .Build();
        }

        private BehaviourTreeStatus DamageAvatar()
        {
            var damage = _novatarConfig.TouchDamage;
            _avatarDamageReceiver.ReceiveDamage(damage);

            _hasDamagedAvatar = true;

            return BehaviourTreeStatus.Success;
        }
    }
}
