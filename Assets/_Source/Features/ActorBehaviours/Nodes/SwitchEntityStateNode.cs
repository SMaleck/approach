using _Source.Entities.Novatar;
using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class SwitchEntityStateNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, EntityState, SwitchEntityStateNode> { }

        private readonly RelationshipDataComponent _relationshipDataComponent;
        private readonly EntityState _targetState;

        public SwitchEntityStateNode(
            IActorStateModel actorStateModel,
            EntityState targetState)
        {
            _relationshipDataComponent = actorStateModel.Get<RelationshipDataComponent>();
            _targetState = targetState;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            _relationshipDataComponent.SetRelationship(_targetState);
            return BehaviourTreeStatus.Success;
        }
    }
}
