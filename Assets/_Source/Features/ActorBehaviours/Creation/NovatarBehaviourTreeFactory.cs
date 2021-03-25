using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Data;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Creation
{
    // ToDo V1 BT FRIENDS: Calm down
    // ToDo V1 BT FRIENDS: Leave when health is low, instead of disappearing
    // ToDo V1 BT Move around on the playing field, increase leave time
    // ToDo V1 BT Sometimes move towards player
    public class NovatarBehaviourTreeFactory
    {
        [Inject] private readonly NodeGenerator.Factory _nodeGeneratorFactory;
        [Inject] private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private NodeGenerator _nodeGenerator;

        public BehaviourTree Create(
            IActorStateModel model,
            MovementController movementController)
        {
            _nodeGenerator = _nodeGeneratorFactory.Create(
                model,
                movementController);

            // @formatter:off
            var startNode = new BehaviourTreeBuilder()
                .Parallel(100, 100)
                    .Selector()
                        .Splice(SpawningTree(model))
                        .Splice(UnacquaintedTree(model))
                        .Splice(NeutralTree(model))
                        .Splice(FriendTree(model))
                        .Splice(EnemyTree(model))
                        .End()
                    .End()
                .Build();
            // @formatter:on

            return new BehaviourTree(
                startNode,
                _nodeGenerator.GetGeneratedNodes());
        }

        #region SubTrees

        private IBehaviourTreeNode SpawningTree(IActorStateModel model)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(model, EntityState.Spawning))
                    .Sequence()
                        .Do(LightSwitch())
                        .Do(EnterScreen())
                        .Do(SwitchStateTo(EntityState.Unacquainted))
                        .End()
                    .End()
                .Build();
            // @formatter:on
        }

        private IBehaviourTreeNode UnacquaintedTree(IActorStateModel model)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(model, EntityState.Unacquainted))
                    .Selector()
                        .Sequence()
                            .Do(FollowAvatar())
                            .Do(Move())
                            .Do(FirstTouch())
                            .End()
                        .Sequence()
                            .Do(UnacquaintedTimeout())
                            .Do(SwitchStateTo(EntityState.Neutral))
                            .End()
                        .End()
                    .End()
                .Build();
            // @formatter:on
        }

        private IBehaviourTreeNode NeutralTree(IActorStateModel model)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(model, EntityState.Neutral))
                    .Sequence()
                        .Do(LeaveScreen())
                        .Do(Deactivate())
                        .End()
                    .End()
                .Build();
            // @formatter:on
        }

        private IBehaviourTreeNode FriendTree(IActorStateModel model)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(model, EntityState.Friend))
                    .Selector()
                        .Sequence()
                            .Do(FollowAvatar())
                            .Do(Move())
                            .End()
                        .Sequence()
                            .Do(FriendTimeout())
                            .Do(SwitchStateTo(EntityState.Neutral))
                            .End()
                        .End()
                    .End()
                .Build();
            // @formatter:on
        }

        private IBehaviourTreeNode EnemyTree(IActorStateModel model)
        {
            // @formatter:off
            return new BehaviourTreeBuilder()
                .Sequence()
                    .Condition(t => IsEntityState(model, EntityState.Enemy))
                    .Selector()
                        .Sequence()
                            .Do(FindDamageReceiver())
                            .Do(Damage())
                            .End()
                        .Sequence()
                            .Do(EnemyTimeout())
                            .Do(SwitchStateTo(EntityState.Neutral))
                            .End()
                        .End()
                    .End()
                .Build();
            // @formatter:on
        }

        #endregion

        #region Convenience Methods

        private bool IsEntityState(IActorStateModel model, EntityState status)
        {
            var relationshipDataComponent = model.Get<RelationshipDataComponent>();
            return relationshipDataComponent.Relationship.Value == status;
        }

        private IBehaviourTreeNode Deactivate()
        {
            return _nodeGenerator.DeactivateSelf();
        }

        private IBehaviourTreeNode EnterScreen()
        {
            return _nodeGenerator.EnterScreen();
        }

        private IBehaviourTreeNode LeaveScreen()
        {
            return _nodeGenerator.LeaveScreen();
        }

        private IBehaviourTreeNode FollowAvatar()
        {
            return _nodeGenerator.FollowAvatar();
        }

        private IBehaviourTreeNode Move()
        {
            return _nodeGenerator.Movement();
        }

        private IBehaviourTreeNode FindDamageReceiver()
        {
            return _nodeGenerator.FindDamageReceiver();
        }

        private IBehaviourTreeNode FirstTouch()
        {
            return _nodeGenerator.FirstTouch();
        }

        private IBehaviourTreeNode Damage()
        {
            return _nodeGenerator.Damage();
        }

        private IBehaviourTreeNode LightSwitch()
        {
            return _nodeGenerator.LightSwitch();
        }

        private IBehaviourTreeNode SwitchStateTo(EntityState entityState)
        {
            return _nodeGenerator.SwitchEntityState(entityState);
        }

        private IBehaviourTreeNode IdleTimeout(double timeout)
        {
            return _nodeGenerator.IdleTimeout(timeout);
        }

        private IBehaviourTreeNode EnemyTimeout()
        {
            return IdleTimeout(
                _behaviourTreeConfig.EnemyLeavingTimeoutSeconds);
        }

        private IBehaviourTreeNode FriendTimeout()
        {
            return IdleTimeout(
                _behaviourTreeConfig.MaxSecondsToFallBehind);
        }

        private IBehaviourTreeNode UnacquaintedTimeout()
        {
            return _nodeGenerator.IdleTimeoutRandom(
                _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds,
                _behaviourTreeConfig.UnacquaintedConfig.TimeBasedSwitchChance);
        }

        #endregion
    }
}
