using _Source.Entities.Novatar;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Nodes
{
    public class LightSwitchNode : AbstractNode, IResettableNode
    {
        public class Factory : PlaceholderFactory<INovatar, LightSwitchNode> { }

        private readonly INovatar _novatarEntity;

        private bool _isLightOn;

        public LightSwitchNode(
            INovatar novatarEntity)
        {
            _novatarEntity = novatarEntity;
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_isLightOn)
            {
                _novatarEntity.TurnLightsOn();
                _isLightOn = true;
            }

            return BehaviourTreeStatus.Success;
        }

        public void Reset()
        {
            _isLightOn = false;
        }
    }
}
