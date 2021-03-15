using _Source.Entities.Novatar;
using _Source.Features.ActorBehaviours.Data;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ActorSensors;
using _Source.Features.Movement;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Creation
{
    public class NovatarBehaviourTreeFactory
    {
        [Inject] private readonly NodeGenerator.Factory _nodeGeneratorFactory;
        [Inject] private readonly BehaviourTreeConfig _behaviourTreeConfig;

        private NodeGenerator _nodeGenerator;

        public BehaviourTree Create(
            IActorStateModel model,
            ISensorySystem sensorySystem,
            MovementController movementController)
        {
            _nodeGenerator = _nodeGeneratorFactory.Create(
                model,
                sensorySystem,
                movementController);

            var unacquaintedRandomTimeoutNode = _nodeGenerator.IdleTimeoutRandom(
                _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds,
                _behaviourTreeConfig.UnacquaintedConfig.TimeBasedSwitchChance);

            var unacquaintedFirstTouchNode = _nodeGenerator.FirstTouch();

            var toNeutralStateNode = _nodeGenerator.SwitchEntityState(
                EntityState.Neutral);

            var toUnacquaintedStateNode = _nodeGenerator.SwitchEntityState(
                EntityState.Unacquainted);

            var friendTimeoutNode = _nodeGenerator.IdleTimeout(
                _behaviourTreeConfig.MaxSecondsToFallBehind);

            var enemyTimeoutNode = _nodeGenerator.IdleTimeout(
                _behaviourTreeConfig.EnemyLeavingTimeoutSeconds);

            var deactivateSelfNode = _nodeGenerator.DeactivateSelf();
            var leaveScreenNode = _nodeGenerator.LeaveScreen();
            var lightSwitchNode = _nodeGenerator.LightSwitch();

            // @formatter:off
            var startNode = new BehaviourTreeBuilder()
                .Parallel(100, 100)
                    .Selector()
                        .Sequence()
                            .Condition(t => IsEntityState(model, EntityState.Spawning))
                            .Sequence()
                                .Do(lightSwitchNode.Tick)
                                .Do(EnterScreen())
                                .Do(toUnacquaintedStateNode.Tick)
                                .End()
                            .End()
                        .Sequence()
                            .Condition(t => IsEntityState(model, EntityState.Unacquainted))
                            .Selector()
                                .Sequence()
                                    .Do(FollowAvatar())
                                    .Do(Move())
                                    .Do(unacquaintedFirstTouchNode.Tick)
                                    .End()
                                .Sequence()
                                    .Do(unacquaintedRandomTimeoutNode.Tick)
                                    .Do(toNeutralStateNode.Tick)
                                    .End()
                                .End()
                            .End()
                        .Sequence()
                            .Condition(t => IsEntityState(model, EntityState.Neutral))
                            .Sequence()
                                .Do(leaveScreenNode.Tick)
                                .Do(deactivateSelfNode.Tick)
                                .End()
                            .End()
                        .Sequence()
                            .Condition(t => IsEntityState(model, EntityState.Friend))
                            .Selector()
                                .Sequence()
                                    .Do(FollowAvatar())
                                    .Do(Move())
                                    .End()
                                .Sequence()
                                    .Do(friendTimeoutNode.Tick)
                                    .Do(toNeutralStateNode.Tick)
                                    .End()
                                .End()
                            .End()
                        .Sequence()
                            .Condition(t => IsEntityState(model, EntityState.Enemy))
                            .Selector()
                                .Sequence()
                                    .Do(FindDamageReceiver())
                                    .Do(Damage())
                                    .Do(enemyTimeoutNode.Tick)
                                    .Do(toNeutralStateNode.Tick)
                                    .End()
                                .Sequence()
                                    .Do(FollowAvatar())
                                    .Do(Move())
                                    .End()
                                .End()
                            .End()
                    .End()
                .End()
                .Build();
            // @formatter:on

            return new BehaviourTree(
                startNode,
                _nodeGenerator.GetGeneratedNodes());
        }

        private bool IsEntityState(IActorStateModel model, EntityState status)
        {
            var relationshipDataComponent = model.Get<RelationshipDataComponent>();
            return relationshipDataComponent.Relationship.Value == status;
        }

        private IBehaviourTreeNode EnterScreen()
        {
            return _nodeGenerator.EnterScreen();
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

        private IBehaviourTreeNode Damage()
        {
            return _nodeGenerator.Damage();
        }
    }
}
