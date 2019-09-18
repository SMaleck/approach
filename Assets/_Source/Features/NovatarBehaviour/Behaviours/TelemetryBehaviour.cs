using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Behaviours
{
    public class TelemetryBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<INovatar, INovatarStateModel, TelemetryBehaviour> { }

        private readonly IAvatar _avatarEntity;

        private readonly IBehaviourTreeNode _behaviourTree;
        private EntityState _lastTrackedRelationShipStatus;

        public TelemetryBehaviour(
            INovatar novatarEntity,
            INovatarStateModel novatarStateModel,
            IAvatar avatarEntity) 
            : base(novatarEntity, novatarStateModel)
        {
            _avatarEntity = avatarEntity;

            _behaviourTree = CreateTree();
        }

        public override IBehaviourTreeNode Build()
        {
            return _behaviourTree;
        }

        private IBehaviourTreeNode CreateTree()
        {
            return new BehaviourTreeBuilder()
                .Parallel(nameof(TelemetryBehaviour), 1, 1)                    
                    .Do(nameof(CalculateDistanceToAvatar), t => CalculateDistanceToAvatar())
                .End()
                .Build();
        }

        private BehaviourTreeStatus CalculateDistanceToAvatar()
        {
            var sqrDistance = NovatarEntity.GetSquaredDistanceTo(_avatarEntity);
            NovatarEntity.SetCurrentDistanceToAvatar(sqrDistance);

            return BehaviourTreeStatus.Success;
        }
    }
}
