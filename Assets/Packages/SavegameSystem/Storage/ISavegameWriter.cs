using Packages.SavegameSystem.Models;

namespace Packages.SavegameSystem.Storage
{
    public interface ISavegameWriter
    {
        void Write(ISavegameData savegameData);
    }
}
