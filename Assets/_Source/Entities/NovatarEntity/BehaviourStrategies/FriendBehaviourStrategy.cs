using _Source.Util;
using Zenject;

namespace _Source.Entities.NovatarEntity.BehaviourStrategies
{
    public class FriendBehaviourStrategy : AbstractDisposable, IBehaviourStrategy
    {
        public class Factory : PlaceholderFactory<Novatar, FriendBehaviourStrategy> { }


        private readonly Novatar _novatar;
        private readonly FriendBehaviourStrategyConfig _friendBehaviourStrategyConfig;
        
        public FriendBehaviourStrategy(
            Novatar novatar,
            FriendBehaviourStrategyConfig friendBehaviourStrategyConfig)
        {
            _novatar = novatar;
            _friendBehaviourStrategyConfig = friendBehaviourStrategyConfig;
        }

        public BehaviourStrategyType StrategyType => BehaviourStrategyType.Friend;
        public void ExecuteLateUpdate()
        {
            
        }
    }
}
