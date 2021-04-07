using Packages.SavegameSystem.Models;

namespace Packages.SavegameSystem.Storage
{
    public interface ISavegameReader
    {
        ISavegameData Read();
    }
}
