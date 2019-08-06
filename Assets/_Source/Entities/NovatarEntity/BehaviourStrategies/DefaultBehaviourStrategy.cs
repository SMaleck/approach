using _Source.Util;
using Zenject;

namespace _Source.Entities.NovatarEntity.BehaviourStrategies
{
    public class DefaultBehaviourStrategy : AbstractDisposable, IBehaviourStrategy
    {
        public class Factory : PlaceholderFactory<Novatar, DefaultBehaviourStrategy> { }


        private readonly Novatar _novatar;
        private readonly Avatar _avatar;
        private readonly DefaultBehaviourStrategyConfig _defaultStrategyConfig;

        public DefaultBehaviourStrategy(
            Novatar novatar,
            Avatar avatar,
            DefaultBehaviourStrategyConfig defaultStrategyConfig)
        {
            _novatar = novatar;
            _avatar = avatar;
            _defaultStrategyConfig = defaultStrategyConfig;
        }

        public BehaviourStrategyType StrategyType => BehaviourStrategyType.Default;

        public void ExecuteLateUpdate()
        {
            var heading = _avatar.HeadingTo(_novatar);
            var sqrDistance = heading.sqrMagnitude;

            var isInRange = sqrDistance <= _novatar.SqrRange;
            var isTouching = sqrDistance <= _novatar.SqrTargetReachedThreshold;

            if (!isInRange || isTouching)
            {
                return;
            }

            _novatar.FollowAvatar();
        }
    }
}
