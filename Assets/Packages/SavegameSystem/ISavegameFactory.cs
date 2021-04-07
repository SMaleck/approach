using Packages.SavegameSystem.Models;

namespace Packages.SavegameSystem
{
    public interface ISavegameFactory
    {
        ISavegameData Create();
    }
}
