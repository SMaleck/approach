using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class LightSwitchNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, LightSwitchNode> { }

        private readonly LightDataComponent _lightDataComponent;

        public LightSwitchNode(IActorStateModel actorStateModel)
        {
            _lightDataComponent = actorStateModel.Get<LightDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (!_lightDataComponent.IsOn.Value)
            {
                _lightDataComponent.TurnLightsOn();
            }

            return BehaviourTreeStatus.Success;
        }
    }
}
