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

            var unacquaintedRandomTimeoutNode = _nodeGenerator.CreateIdleTimeoutRandomNode(
                _behaviourTreeConfig.UnacquaintedConfig.EvaluationTimeoutSeconds,
                _behaviourTreeConfig.UnacquaintedConfig.TimeBasedSwitchChance);

            var unacquaintedFirstTouchNode = _nodeGenerator.CreateFirstTouchNode();

            var toNeutralStateNode = _nodeGenerator.CreateSwitchEntityStateNode(
                EntityState.Neutral);

            var toUnacquaintedStateNode = _nodeGenerator.CreateSwitchEntityStateNode(
                EntityState.Unacquainted);

            var friendTimeoutNode = _nodeGenerator.CreateIdleTimeoutNode(
                _behaviourTreeConfig.MaxSecondsToFallBehind);

            var enemyTimeoutNode = _nodeGenerator.CreateIdleTimeoutNode(
                _behaviourTreeConfig.EnemyLeavingTimeoutSeconds);

            var deactivateSelfNode = _nodeGenerator.CreateDeactivateSelfNode();
            var leaveScreenNode = _nodeGenerator.CreateLeaveScreenNode();
            var damageAvatarNode = _nodeGenerator.CreateDamageAvatarNode();
            var lightSwitchNode = _nodeGenerator.CreateLightSwitchNode();

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
                                    .Do(damageAvatarNode.Tick)
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
            return _nodeGenerator.CreateEnterScreenNode();
        }

        private IBehaviourTreeNode FollowAvatar()
        {
            return _nodeGenerator.CreateFollowAvatarNode();
        }

        private IBehaviourTreeNode Move()
        {
            return _nodeGenerator.CreateMovementNode();
        }
    }
}
