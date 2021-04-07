using Packages.SavegameSystem.Models;

namespace Packages.SavegameSystem
{
    public interface ISavegameService
    {
        ISavegameData Load();

        void EnqueueSaveRequest();
        void Save();

        void Reset();
    }
}
