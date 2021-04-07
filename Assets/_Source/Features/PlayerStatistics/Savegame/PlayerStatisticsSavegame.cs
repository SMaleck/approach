using _Source.Services.SavegameSystem.Models;
using UniRx;

namespace _Source.Features.PlayerStatistics.Savegame
{
    public class PlayerStatisticsSavegame : AbstractSavegame, IPlayerStatisticsSavegame
    {
        public IReactiveProperty<int> RoundsPlayed { get; }

        public PlayerStatisticsSavegame(PlayerStatisticsSavegameData savegameData)
        {
            RoundsPlayed = CreateBoundProperty(
                savegameData.RoundsPlayed,
                value => savegameData.RoundsPlayed = value);
        }
    }
}
