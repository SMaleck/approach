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
        private static readonly string[] BuildScenes =
        {
            "Assets/Scenes/InitScene.unity",
            "Assets/Scenes/TitleScene.unity" ,
            "Assets/Scenes/GameScene.unity"
        };

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
            return new BuildPlayerOptions()
            {
                scenes = BuildScenes,
                locationPathName = ApkName,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };
        }

        public static void LogBuildOptions()
        {
            Debug.Log("\n-------- Build Options --------");
            Debug.Log($"APK Name: {ApkName}");
            Debug.Log("Scenes:");
            BuildScenes.ToList().ForEach(Debug.Log);
            Debug.Log("-------- Build Options --------\n");
        }
    }
}
