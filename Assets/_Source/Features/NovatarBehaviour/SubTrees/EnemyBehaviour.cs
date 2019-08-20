using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.AvatarState;
using _Source.Features.NovatarBehaviour.Data;
using FluentBehaviourTree;
using UniRx;
using Zenject;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public class EnemyBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<NovatarEntity, NovatarStateModel, EnemyBehaviour> { }

        private readonly BehaviourTreeConfig _behaviourTreeConfig;
        private readonly NeutralBehaviour.Factory _neutralBehaviourFactory;
        private readonly NovatarConfig _novatarConfig;
        private readonly IDamageReceiver _avatarDamageReceiver;
        private readonly AvatarEntity _avatar;

        private readonly IBehaviourTreeNode _behaviourTree;

        private bool _hasDamagedAvatar = false;
        private double _timeSinceHasDamagedPlayerSeconds = 0;

        public EnemyBehaviour(
            NovatarEntity novatarEntity,
            NovatarStateModel novatarStateModel,
            BehaviourTreeConfig behaviourTreeConfig,
            NeutralBehaviour.Factory neutralBehaviourFactory,
            NovatarConfig novatarConfig,
            IDamageReceiver avatarDamageReceiver)
            : base(novatarEntity, novatarStateModel)
        {
            _behaviourTreeConfig = behaviourTreeConfig;
            _neutralBehaviourFactory = neutralBehaviourFactory;
            _novatarConfig = novatarConfig;
            _avatarDamageReceiver = avatarDamageReceiver;

            _behaviourTree = CreateTree();

            NovatarStateModel.OnReset
                .Subscribe(_ => Reset())
                .AddTo(Disposer);
        }

        private void Reset()
        {
            _hasDamagedAvatar = false;
            _timeSinceHasDamagedPlayerSeconds = 0;
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
                .Parallel(nameof(EnemyBehaviour), 3, 3)
                    .Do(nameof(TrackTimeSinceHasDamagedPlayer), TrackTimeSinceHasDamagedPlayer)
                    .Selector("EnemyBehaviourSelector")
                        .Sequence("Neutral")
                            .Condition(nameof(ShouldLeave), t => ShouldLeave())
                            .Splice(neutralBehaviour)
                            .End()
                        .Sequence("DamageAvatar")
                            .Condition(nameof(IsInTouchRange), t => IsInTouchRange())
                            .Do(nameof(DamageAvatar), t => DamageAvatar())
                            .End()
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

        private bool ShouldLeave()
        {
            return _hasDamagedAvatar && _timeSinceHasDamagedPlayerSeconds >= _behaviourTreeConfig.EnemyLeavingTimeoutSeconds;
        }

        private BehaviourTreeStatus TrackTimeSinceHasDamagedPlayer(TimeData timeData)
        {
            if (_hasDamagedAvatar)
            {
                _timeSinceHasDamagedPlayerSeconds += timeData.deltaTime;
            }

            return BehaviourTreeStatus.Success;
        }
    }
}
