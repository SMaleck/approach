using _Source.Data;

namespace _Source.Features.ActorBehaviours.Data
{
    public class StateDelaysRepository : AbstractDataRepository, IStateDelaysData
    {
        private readonly StateDelayDataEntry _dataEntry;

        public double EnemyStaySeconds => _dataEntry.EnemyStaySeconds;
        public double FriendPatienceSeconds => _dataEntry.FriendPatienceSeconds;
        public double NeutralStaySeconds => _dataEntry.NeutralStaySeconds;

        public StateDelaysRepository(StateDelaysDataSource dataSource)
        {
            _dataEntry = dataSource.DataEntries[0];
        }
    }
}
