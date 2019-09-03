using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using FluentBehaviourTree;
using Zenject;

namespace _Source.Features.NovatarBehaviour.Behaviours
{
    public class TelemetryBehaviour : AbstractBehaviour
    {
        public class Factory : PlaceholderFactory<INovatar, NovatarStateModel, TelemetryBehaviour> { }

        private readonly IAvatar _avatarEntity;

        private readonly IBehaviourTreeNode _behaviourTree;
        private RelationshipStatus _lastTrackedRelationShipStatus;

        public TelemetryBehaviour(
            INovatar novatarEntity,
            NovatarStateModel novatarStateModel,
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
                    .Do(nameof(TrackTimePassedInCurrentStatus), TrackTimePassedInCurrentStatus)
                    .Do(nameof(CalculateDistanceToAvatar), t => CalculateDistanceToAvatar())
                .End()
                .Build();
        }

        private BehaviourTreeStatus TrackTimePassedInCurrentStatus(TimeData timeData)
        {
            var currentStatus = NovatarStateModel.CurrentRelationshipStatus.Value;
            var currentTimePassed = NovatarStateModel.TimePassedInCurrentStatusSeconds.Value;

            var hasStatusChanged = currentStatus != _lastTrackedRelationShipStatus;
            var trackedTime = hasStatusChanged
                ? 0
                : currentTimePassed + timeData.deltaTime;

            NovatarStateModel.SetTimePassedInCurrentStatusSeconds(trackedTime);
            _lastTrackedRelationShipStatus = currentStatus;

            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus CalculateDistanceToAvatar()
        {
            var sqrDistance = NovatarEntity.GetSquaredDistanceTo(_avatarEntity);
            NovatarStateModel.SetCurrentDistanceToAvatar(sqrDistance);

            return BehaviourTreeStatus.Success;
        }
    }
}
