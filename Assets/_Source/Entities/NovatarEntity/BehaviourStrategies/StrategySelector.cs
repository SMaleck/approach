﻿using _Source.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace _Source.Entities.NovatarEntity.BehaviourStrategies
{
    public class StrategySelector
    {
        public class Factory : PlaceholderFactory<Novatar, StrategySelector> { }

        private readonly Novatar _novatar;
        private readonly DefaultBehaviourStrategy.Factory _defaultStrategyFactory;
        private readonly FriendBehaviourStrategy.Factory _friendStrategyFactory;

        private readonly List<IBehaviourStrategy> _behaviourStrategies;

        public StrategySelector(
            Novatar novatar,
            DefaultBehaviourStrategy.Factory defaultStrategyFactory,
            FriendBehaviourStrategy.Factory friendStrategyFactory)
        {
            _novatar = novatar;
            _defaultStrategyFactory = defaultStrategyFactory;
            _friendStrategyFactory = friendStrategyFactory;

            _behaviourStrategies = CreateStrategies();
        }

        private List<IBehaviourStrategy> CreateStrategies()
        {
            return EnumHelper<BehaviourStrategyType>.Iterator
                .Select(CreateBehaviourStrategy)
                .ToList();
        }

        private IBehaviourStrategy CreateBehaviourStrategy(BehaviourStrategyType strategyType)
        {
            switch (strategyType)
            {
                case BehaviourStrategyType.Default:
                    return _defaultStrategyFactory.Create(_novatar);

                case BehaviourStrategyType.Friend:
                    return _friendStrategyFactory.Create(_novatar);

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategyType), strategyType, null);
            }
        }

        public void SwitchTo(BehaviourStrategyType strategyTypeToActivate)
        {
            _behaviourStrategies.ForEach(strategy =>
            {
                if (strategy.StrategyType != strategyTypeToActivate)
                {
                    strategy.Deactivate();
                    return;
                }

                strategy.Activate();
            });
        }
    }
}
