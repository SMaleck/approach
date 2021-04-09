using _Source.App;
using _Source.Data;
using UnityEngine;

namespace _Source.Features.ActorBehaviours.Data
{
    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(StateDelaysDataSource), fileName = nameof(StateDelaysDataSource))]
    public class StateDelaysDataSource : AbstractDataSource<StateDelayDataEntry>
    {
    }
}
