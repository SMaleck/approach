using _Source.Entities.Novatar;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class SwitchEntityStateNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<INovatar, EntityState, SwitchEntityStateNode> { }

        private readonly INovatar _novatarEntity;
        private readonly EntityState _targetState;

        public SwitchEntityStateNode(
            INovatar novatarEntity,
            EntityState targetState)
        {
            _novatarEntity = novatarEntity;
            _targetState = targetState;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            _novatarEntity.SwitchToEntityState(_targetState);
            return BehaviourTreeStatus.Success;
        }
    }
}
