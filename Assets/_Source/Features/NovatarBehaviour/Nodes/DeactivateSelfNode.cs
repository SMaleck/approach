using _Source.Entities.Novatar;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class DeactivateSelfNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<INovatar, DeactivateSelfNode> { }

        private readonly INovatar _novatarEntity;

        public DeactivateSelfNode(INovatar novatarEntity)
        {
            _novatarEntity = novatarEntity;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            _novatarEntity.Deactivate();
            return BehaviourTreeStatus.Success;
        }
    }
}
