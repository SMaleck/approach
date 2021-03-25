using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class NearDeathNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, NearDeathNode> { }

        private readonly HealthDataComponent _healthDataComponent;

        public NearDeathNode(IActorStateModel actorStateModel)
        {
            _healthDataComponent = actorStateModel.Get<HealthDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            if (_healthDataComponent.Health.Value <= 1)
            {
                return BehaviourTreeStatus.Success;
            }

            return BehaviourTreeStatus.Failure;
        }
    }
}
