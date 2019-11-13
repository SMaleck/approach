using System.IO;
using UnityEngine;

namespace Assets.Editor.CiBuild.Config
{
    public static class BuildConfigReader
    {
        private const string BuildConfigFileName = "build_config.json";

        public static BuildConfig Read()
        {
            var projectRootPath = Directory.GetParent(Application.dataPath).FullName;
            var configFilePath = Path.Combine(projectRootPath, BuildConfigFileName);
            if (!File.Exists(configFilePath))
            {
                Debug.LogWarning($"No such config file: {configFilePath}");
                return default(BuildConfig);
            }

            var jsonContent = File.ReadAllText(configFilePath);

            Debug.Log($"Read BuildConfig file from: {configFilePath}");
            Debug.Log(jsonContent);

            return JsonUtility.FromJson<BuildConfig>(jsonContent);
        }
    }
}
