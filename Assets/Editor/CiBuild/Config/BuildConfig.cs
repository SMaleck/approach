using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CiBuild.Config
{
    public static class BuildConfig
    {
        private const string EnvironmentConfigFileName = "build_config.json";
        private const string ApkName = "approach.apk";

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

        public static BuildPlayerOptions GetBuildPlayerOptions()
        {
            Debug.Log("\n\n----------------- Generating Build Options\n");

            Debug.Log($"APK Name: {ApkName}");
            Debug.Log("Scenes:");
            var scenePaths = GetScenePaths();
            scenePaths.ToList().ForEach(Debug.Log);

            return new BuildPlayerOptions()
            {
                scenes = scenePaths,
                locationPathName = ApkName,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };
        }

        private static string[] GetScenePaths()
        {
            return EditorBuildSettings.scenes
                .ToList()
                .Select(scene => scene.path)
                .ToArray();
        }
    }
}
