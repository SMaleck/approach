using _Source.Features.Actors.Data;
using Zenject;

namespace _Source.Features.Actors.DataComponents
{
    public class WanderDataComponent : AbstractDataComponent
    {
        public class Factory : PlaceholderFactory<IWanderData, WanderDataComponent> { }

        private readonly IWanderData _wanderData;

        public float WanderMinDistance => _wanderData.WanderMinDistance;
        public float WanderMaxDistance => _wanderData.WanderMaxDistance;

        public WanderDataComponent(IWanderData wanderData)
        {
            _wanderData = wanderData;
        }
    }
}
