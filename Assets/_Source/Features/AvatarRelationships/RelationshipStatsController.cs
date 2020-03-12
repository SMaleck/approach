namespace _Source.Features.AvatarRelationships
{
    public class RelationshipStatsController
    {
        private readonly RelationshipStatsModel _relationshipStatsModel;

        public RelationshipStatsController(RelationshipStatsModel relationshipStatsModel)
        {
            _relationshipStatsModel = relationshipStatsModel;
        }

        public void IncrementEncountersCount()
        {
            _relationshipStatsModel.IncrementEncountersCount();
        }

        public void IncrementFriendsGainedCount()
        {
            _relationshipStatsModel.IncrementFriendsGainedCount();
        }

        public void IncrementFriendsLostCount()
        {
            _relationshipStatsModel.IncrementFriendsLostCount();
        }

        public void IncrementEnemiesGainedCount()
        {
            _relationshipStatsModel.IncrementEnemiesGainedCount();
        }
    }
}
