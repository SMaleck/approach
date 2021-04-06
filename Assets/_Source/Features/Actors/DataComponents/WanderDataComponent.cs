using _Source.Features.Actors.Data;
using _Source.Features.Movement.Data;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class WanderDataComponent : AbstractDataComponent
    {
        public class Factory : PlaceholderFactory<IWanderData, WanderDataComponent> { }

        private readonly IWanderData _wanderData;

        public float WanderMinDistance => _wanderData.MinDistance;
        public float WanderMaxDistance => _wanderData.MaxDistance;

        public WanderDataComponent(IWanderData wanderData)
        {
            _wanderData = wanderData;
        }
    }
}
