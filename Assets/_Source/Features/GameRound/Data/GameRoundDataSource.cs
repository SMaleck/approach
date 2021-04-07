using _Source.App;
using _Source.Data;
using UnityEngine;

namespace _Source.Features.GameRound.Data
{
    [CreateAssetMenu(menuName = Constants.DataMenu + nameof(GameRoundDataSource), fileName = nameof(GameRoundDataSource))]
    public class GameRoundDataSource : AbstractDataSource<GameRoundDataEntry>
    {
    }
}
