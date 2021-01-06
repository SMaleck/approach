using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class DeactivateSelfNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<IActorStateModel, DeactivateSelfNode> { }

        private readonly HealthDataComponent _healthDataComponent;

        public DeactivateSelfNode(IActorStateModel actorStateModel)
        {
            _healthDataComponent = actorStateModel.Get<HealthDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            _healthDataComponent.SetIsAlive(false);
            return BehaviourTreeStatus.Success;
        }
    }
}
