using _Source.Util;
using Zenject;

namespace _Source.Entities.NovatarEntity.BehaviourStrategies
{
    public class DefaultBehaviourStrategy : AbstractDisposable, IBehaviourStrategy
    {
        public class Factory : PlaceholderFactory<Novatar, DefaultBehaviourStrategy> { }


        private readonly Novatar _novatar;
        private readonly DefaultBehaviourStrategyConfig _defaultStrategyConfig;
        
        public DefaultBehaviourStrategy(
            Novatar novatar,
            DefaultBehaviourStrategyConfig defaultStrategyConfig)
        {
            _novatar = novatar;
            _defaultStrategyConfig = defaultStrategyConfig;
        }

        public BehaviourStrategyType StrategyType => BehaviourStrategyType.Default;

        public void Activate()
        {

        }

        public void Deactivate()
        {
            
        }
    }
}
