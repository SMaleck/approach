using _Source.Services.SavegameSystem.Models;
using Packages.Logging;
using Packages.SavegameSystem.Config;
using Packages.SavegameSystem.Models;
using Packages.SavegameSystem.Storage;
using System.IO;

namespace _Source.Services.SavegameSystem.Storage
{
    public class SavegameLocalStorage : ISavegameReader, ISavegameWriter
    {
        private readonly ILogger _logger;
        private readonly string _fullFilePath;

        public SavegameLocalStorage(
            ILogger logger,
            ISavegameConfig savegameConfig)
        {
            _logger = logger;
            var basePath = UnityEngine.Application.persistentDataPath;
            _fullFilePath = Path.Combine(basePath, savegameConfig.Filename);
        }

        public ISavegameData Read()
        {
            _logger.Log($"Loading savegame from {_fullFilePath}");
            var savegameData = JsonStorage.Read<SavegameData>(_fullFilePath);

            return savegameData;
        }

        public void Write(ISavegameData savegameData)
        {
            _logger.Log($"Saving savegame to {_fullFilePath}");
            JsonStorage.Write(_fullFilePath, savegameData);
        }
    }
}
