using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using _Source.Features.Movement;
using _Source.Features.NovatarBehaviour.Data;
using FluentBehaviourTree;
using UniRx;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Behaviours
{
    public class EnemyBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, MovementController , EnemyBehaviour> { }

        private readonly MovementController _movementController;
        private readonly BehaviourTreeConfig _behaviourTreeConfig;
        private readonly NeutralBehaviour.Factory _neutralBehaviourFactory;
        private readonly NovatarConfig _novatarConfig;
        private readonly IDamageReceiver _avatarDamageReceiver;

        private readonly IBehaviourTreeNode _behaviourTree;

        private bool _hasDamagedAvatar = false;
        private double _timeSinceHasDamagedPlayerSeconds = 0;

        public EnemyBehaviour(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            MovementController movementController,
            BehaviourTreeConfig behaviourTreeConfig,
            NeutralBehaviour.Factory neutralBehaviourFactory,
            NovatarConfig novatarConfig,
            IDamageReceiver avatarDamageReceiver)
            : base(novatarEntity, novatarStateModel)
        {
            _movementController = movementController;
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
                    NovatarStateModel,
                    _movementController)
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
            if (!_hasDamagedAvatar)
            {
                var damage = _novatarConfig.TouchDamage;
                _avatarDamageReceiver.ReceiveDamage(damage);

                _hasDamagedAvatar = true;
            }

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
