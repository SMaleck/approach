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

        public BehaviourTree Create(
            IActorStateModel model,
            ISensorySystem sensorySystem,
            MovementController movementController)
        {
            var nodeGenerator = _nodeGeneratorFactory.Create(
                model,
                sensorySystem,
                movementController);

            var unacquaintedRandomTimeoutNode = nodeGenerator.CreateIdleTimeoutRandomNode(
                _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds,
                _behaviourTreeConfig.UnacquaintedConfig.TimeBasedSwitchChance);

            var unacquaintedFirstTouchNode = nodeGenerator.CreateFirstTouchNode();

            var toNeutralStateNode = nodeGenerator.CreateSwitchEntityStateNode(
                EntityState.Neutral);

            var toUnacquaintedStateNode = nodeGenerator.CreateSwitchEntityStateNode(
                EntityState.Unacquainted);

            var friendTimeoutNode = nodeGenerator.CreateIdleTimeoutNode(
                _behaviourTreeConfig.MaxSecondsToFallBehind);

            var enemyTimeoutNode = nodeGenerator.CreateIdleTimeoutNode(
                _behaviourTreeConfig.EnemyLeavingTimeoutSeconds);

            var followAvatarNode = nodeGenerator.CreateFollowAvatarNode();
            var deactivateSelfNode = nodeGenerator.CreateDeactivateSelfNode();
            var leaveScreenNode = nodeGenerator.CreateLeaveScreenNode();
            var damageAvatarNode = nodeGenerator.CreateDamageAvatarNode();
            var lightSwitchNode = nodeGenerator.CreateLightSwitchNode();
            var enterScreenNode = nodeGenerator.CreateEnterScreenNode();
            var movementNode = nodeGenerator.CreateMovementNode();

            // @formatter:off
            var startNode = new BehaviourTreeBuilder()
                .Parallel(100, 100)
                    .Selector()
                        .Sequence()
                            .Condition(t => IsEntityState(model, EntityState.Spawning))
                            .Sequence()
                                .Do(lightSwitchNode.Tick)
                                .Do(enterScreenNode.Tick)
                                .Do(toUnacquaintedStateNode.Tick)
                                .End()
                            .End()
                        .Sequence()
                            .Condition(t => IsEntityState(model, EntityState.Unacquainted))
                            .Selector()
                                .Sequence()
                                    .Do(followAvatarNode.Tick) // TODO Problem
                                    .Do(movementNode.Tick)
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
                                    .Do(followAvatarNode.Tick)
                                    .Do(movementNode.Tick)
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
                                    .Do(damageAvatarNode.Tick)
                                    .Do(enemyTimeoutNode.Tick)
                                    .Do(toNeutralStateNode.Tick)
                                    .End()
                                .Sequence()
                                    .Do(followAvatarNode.Tick)
                                    .Do(movementNode.Tick)
                                    .End()
                                .End()
                            .End()
                    .End()
                .End()
                .Build();
            // @formatter:on

            return new BehaviourTree(
                startNode,
                nodeGenerator.GetGeneratedNodes());
        }

        private bool IsEntityState(IActorStateModel model, EntityState status)
        {
            var relationshipDataComponent = model.Get<RelationshipDataComponent>();
            return relationshipDataComponent.Relationship.Value == status;
        }
    }
}
