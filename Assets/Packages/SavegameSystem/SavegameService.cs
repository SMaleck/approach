using Packages.Logging;
using Packages.RxUtils;
using Packages.SavegameSystem.Config;
using Packages.SavegameSystem.Models;
using Packages.SavegameSystem.Storage;
using System;
using UniRx;

namespace Packages.SavegameSystem
{
    public class SavegameService : AbstractDisposable, ISavegameService
    {
        private readonly ILogger _logger;
        private readonly ISavegameFactory _savegameFactory;
        private readonly ISavegameConfig _savegameConfig;
        private readonly ISavegameReader _savegameReader;
        private readonly ISavegameWriter _savegameWriter;

        private readonly SerialDisposable _saveDisposer;
        private readonly TimeSpan _requestSaveTimeout;

        private ISavegameData _savegameData;

        public SavegameService(
            ILogger logger,
            ISavegameFactory savegameFactory,
            ISavegameConfig savegameConfig,
            ISavegameReader savegameReader,
            ISavegameWriter savegameWriter)
        {
            _logger = logger;
            _savegameFactory = savegameFactory;
            _savegameConfig = savegameConfig;
            _savegameReader = savegameReader;
            _savegameWriter = savegameWriter;

            _saveDisposer = new SerialDisposable().AddTo(Disposer);
            _requestSaveTimeout = TimeSpan.FromSeconds(_savegameConfig.RequestSaveTimeoutSeconds);
        }

        public ISavegameData Load()
        {
            _savegameData = _savegameReader.Read();
            _savegameData = _savegameData ?? _savegameFactory.Create();

            return _savegameData;
        }

        public void EnqueueSaveRequest()
        {
            _logger.Log($"Savegame dirty, saving in {_requestSaveTimeout.TotalSeconds}s");

            _saveDisposer.Disposable = Observable.Timer(_requestSaveTimeout)
                .Subscribe(_ => Save());
        }

        public void Save()
        {
            _saveDisposer.Disposable?.Dispose();
            _savegameWriter.Write(_savegameData);
        }

        void ISavegameService.Reset()
        {
            _savegameData = _savegameFactory.Create();
            Save();
        }
    }
}
