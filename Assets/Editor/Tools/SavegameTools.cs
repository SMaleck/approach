using _Source.App;
using Packages.SavegameSystem.Config;
using System.IO;
using UnityEditor;

namespace Assets.Editor.Tools
{
    public static class SavegameTools
    {
        private const string Menu = Constants.MenuBase + "/Savegame/";

        [MenuItem(Menu + "Clear")]
        private static void Clear()
        {
            var savegameConfig = AssetLoader.LoadByType<SavegamesConfig>();

            var filePath = Path.Combine(
                UnityEngine.Application.persistentDataPath,
                savegameConfig.Filename);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
