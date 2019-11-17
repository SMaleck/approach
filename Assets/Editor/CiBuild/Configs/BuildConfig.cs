using System.IO;
using UnityEngine;

namespace Assets.Editor.CiBuild.Configs
{
    public static class BuildConfig
    {
        public static EnvironmentConfig ReadEnvironmentConfig()
        {
            var projectRootPath = Directory.GetParent(Application.dataPath).FullName;
            var configFilePath = Path.Combine(projectRootPath, EnvironmentConfig.FileName);
            if (!File.Exists(configFilePath))
            {
                Debug.LogWarning($"No such config file: {configFilePath}");
                return default(EnvironmentConfig);
            }

            var jsonContent = File.ReadAllText(configFilePath);

            Debug.Log($"Read EnvironmentConfig file from: {configFilePath}");
            Debug.Log(jsonContent);

            return JsonUtility.FromJson<EnvironmentConfig>(jsonContent);
        }

        public static ProjectConfig ReadProjectConfig()
        {
            var projectRootPath = Directory.GetParent(Application.dataPath).FullName;
            var configFilePath = Path.Combine(projectRootPath, ProjectConfig.FileName);
            if (!File.Exists(configFilePath))
            {
                Debug.LogWarning($"No such config file: {configFilePath}");
                return default(ProjectConfig);
            }

            var jsonContent = File.ReadAllText(configFilePath);

            Debug.Log($"Read ProjectConfig file from: {configFilePath}");
            Debug.Log(jsonContent);

            return JsonUtility.FromJson<ProjectConfig>(jsonContent);
        }
    }
}
