using Packages.SavegameSystem.Utility;
using UnityEngine;

namespace Packages.SavegameSystem.Config
{
    [CreateAssetMenu(fileName = nameof(SavegamesConfig), menuName = MenuConstants.Root + nameof(SavegamesConfig))]
    public class SavegamesConfig : ScriptableObject, ISavegameConfig
    {
        [SerializeField] private string _savegameFilename = "player.sav";
        public string Filename => _savegameFilename;

        [Range(0.1f, 2f)]
        [SerializeField] private double _requestSaveTimeoutSeconds = 1d;
        public double RequestSaveTimeoutSeconds => _requestSaveTimeoutSeconds;
    }
}
