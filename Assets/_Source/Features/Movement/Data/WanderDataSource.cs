using _Source.App;
using _Source.Data;
using System;
using UnityEngine;

namespace _Source.Features.Movement.Data
{
    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(WanderDataSource), fileName = nameof(WanderDataSource))]
    public class WanderDataSource : AbstractDataSource<WanderDataEntry>
    {
    }
}
