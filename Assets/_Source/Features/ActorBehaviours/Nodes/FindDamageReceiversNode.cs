using _Source.Features.Actors;
using _Source.Features.Actors.DataComponents;
using _Source.Features.ActorSensors;
using BehaviourTreeSystem;
using Zenject;

namespace _Source.Features.ActorBehaviours.Nodes
{
    public class FindDamageReceiversNode : AbstractNode
    {
        public class Factory : PlaceholderFactory<ISensorySystem, IActorStateModel, FindDamageReceiversNode> { }

        private readonly ISensorySystem _sensorySystem;
        private readonly IActorStateModel _actorStateModel;
        private readonly BlackBoardDataComponent _blackBoard;

        public FindDamageReceiversNode(
            ISensorySystem sensorySystem, 
            IActorStateModel actorStateModel)
        {
            _sensorySystem = sensorySystem;
            _actorStateModel = actorStateModel;

            _blackBoard = actorStateModel.Get<BlackBoardDataComponent>();
        }

        public override BehaviourTreeStatus Tick(TimeData time)
        {
            return BehaviourTreeStatus.Success;
        }
    }
}
