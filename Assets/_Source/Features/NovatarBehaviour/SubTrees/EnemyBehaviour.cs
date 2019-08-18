using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.AvatarState;
using _Source.Features.GameWorld;
using _Source.Features.NovatarBehaviour.Data;
using FluentBehaviourTree;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public class EnemyBehaviour : AbstractBehaviour
    {
        private readonly NovatarConfig _novatarConfig;
        private readonly IDamageReceiver _avatarDamageReceiver;
        private readonly AvatarEntity _avatar;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;
        private readonly ScreenSizeController _screenSizeController;

        private readonly IBehaviourTreeNode _behaviourTree;

        public EnemyBehaviour(
            NovatarEntity novatarEntity,
            NovatarStateModel novatarStateModel,
            NovatarConfig novatarConfig,
            IDamageReceiver avatarDamageReceiver)
            : base(novatarEntity, novatarStateModel)
        {
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
            return new BehaviourTreeBuilder()
                .Sequence("DamageAvatar")
                        .Condition(nameof(IsInTouchRange), t => IsInTouchRange())
                        .Do(nameof(DamageAvatar), t => DamageAvatar())
                .End()
                .Build();
        }

        private BehaviourTreeStatus DamageAvatar()
        {
            var damage = _novatarConfig.TouchDamage;
            _avatarDamageReceiver.ReceiveDamage(damage);
            DeactivateSelf();

            return BehaviourTreeStatus.Success;
        }

        private void DeactivateSelf()
        {
            NovatarStateModel.SetIsAlive(false);
        }
    }
}
