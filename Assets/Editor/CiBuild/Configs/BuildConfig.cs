using System.IO;
using UnityEngine;

namespace Assets.Editor.CiBuild.Configs
{
    public static class BuildConfig
    {
        private const string EnvironmentConfigFileName = "build_config.json";

        public static EnvironmentConfig ReadEnvironmentConfig()
        {
            var projectRootPath = Directory.GetParent(Application.dataPath).FullName;
            var configFilePath = Path.Combine(projectRootPath, EnvironmentConfigFileName);
            if (!File.Exists(configFilePath))
            {
                Debug.LogWarning($"No such config file: {configFilePath}");
                return default(EnvironmentConfig);
            }

            var jsonContent = File.ReadAllText(configFilePath);

            Debug.Log($"Read BuildConfig file from: {configFilePath}");
            Debug.Log(jsonContent);

            return JsonUtility.FromJson<EnvironmentConfig>(jsonContent);
        }
    }
}
