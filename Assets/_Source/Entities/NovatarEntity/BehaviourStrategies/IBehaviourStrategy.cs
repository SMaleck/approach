namespace _Source.Entities.NovatarEntity.BehaviourStrategies
{
    public interface IBehaviourStrategy
    {
        BehaviourStrategyType StrategyType { get; }

        void ExecuteLateUpdate();
    }
}
