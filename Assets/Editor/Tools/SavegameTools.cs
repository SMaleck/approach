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
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [MenuItem(Menu + "Open Folder")]
        private static void GoTo()
        {
            var filePath = GetFilePath();
            EditorUtility.RevealInFinder(filePath);
        }

        private static string GetFilePath()
        {
            var savegameConfig = AssetLoader.LoadByType<SavegamesConfig>();

            return Path.Combine(
                UnityEngine.Application.persistentDataPath,
                savegameConfig.Filename);
        }
    }
}
