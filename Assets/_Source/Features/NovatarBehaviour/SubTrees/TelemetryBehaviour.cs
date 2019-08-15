using _Source.Entities.Avatar;
using _Source.Entities.Novatar;
using FluentBehaviourTree;

namespace _Source.Features.NovatarBehaviour.SubTrees
{
    public class TelemetryBehaviour
    {
        private const string TreeName = "";

        private readonly NovatarEntity _novatar;
        private readonly NovatarStateModel _novatarStateModel;
        private readonly AvatarEntity _avatar;

        private readonly IBehaviourTreeNode _behaviourTree;
        private RelationshipStatus _lastTrackedRelationShipStatus;

        public TelemetryBehaviour(
            NovatarEntity novatar,
            NovatarStateModel novatarStateModel,
            AvatarEntity avatar)
        {
            _novatar = novatar;
            _novatarStateModel = novatarStateModel;
            _avatar = avatar;

            _behaviourTree = CreateTree();
        }

        public BehaviourTreeStatus Tick(TimeData timeData)
        {
            return _behaviourTree.Tick(timeData);
        }

        private IBehaviourTreeNode CreateTree()
        {
            return new BehaviourTreeBuilder()
                .Parallel(TreeName, 1, 1)
                    .Do(nameof(TrackTimePassedInCurrentStatus), TrackTimePassedInCurrentStatus)
                    .Do(nameof(CalculateDistanceToAvatar), t => CalculateDistanceToAvatar())
                .End()
                .Build();
        }

        private BehaviourTreeStatus TrackTimePassedInCurrentStatus(TimeData timeData)
        {
            var currentStatus = _novatarStateModel.CurrentRelationshipStatus.Value;
            var currentTimePassed = _novatarStateModel.TimePassedInCurrentStatusSeconds.Value;

            var hasStatusChanged = currentStatus != _lastTrackedRelationShipStatus;
            var trackedTime = hasStatusChanged
                ? 0
                : currentTimePassed + timeData.deltaTime;

            _novatarStateModel.SetTimePassedInCurrentStatusSeconds(trackedTime);
            _lastTrackedRelationShipStatus = currentStatus;

            return BehaviourTreeStatus.Success;
        }

        private BehaviourTreeStatus CalculateDistanceToAvatar()
        {
            var sqrDistance = _novatar.GetSquaredDistanceTo(_avatar);
            _novatarStateModel.SetCurrentDistanceToAvatar(sqrDistance);

            return BehaviourTreeStatus.Success;
        }
    }
}
