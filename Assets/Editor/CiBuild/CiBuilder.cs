using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CiBuild
{
    public static class CiBuilder
    {
        private const string BuildConfigFileName = "build_config.json";
        private static BuildConfig BuildConfig;

        private static readonly string[] BuildScenes =
        {
            "Assets/InitScene.unity",
            "Assets/TitleScene.unity" ,
            "Assets/GameScene.unity"
        };

        private const string ApkName = "approach.apk";
        private const string AndroidBuildPath = "androidBuild";

        public static void Run()
        {
            Debug.Log("\n\n----------- Starting Build Script\n");

            Prepare();
            LogSetup();
            RunBuild();
        }

        private static void Prepare()
        {
            Debug.Log("\n\n----------- Preparing Environment\n");

            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);

            BuildConfig = GetBuildConfig();

            EditorPrefs.SetString("AndroidSdkRoot", BuildConfig.AndroidSdkRoot);
            EditorPrefs.SetString("AndroidNdkRoot", BuildConfig.AndroidNdkRoot);
        }

        private static void LogSetup()
        {
            Debug.Log($"APK Path: {ApkName}");
            Debug.Log("Scenes:");
            foreach (var scene in BuildScenes)
            {
                Debug.Log(scene);
            }
        }

        private static void RunBuild()
        {
            Debug.Log("\n----------- Starting BuildPipeline for Android");

            BuildPipeline.BuildPlayer(
                BuildScenes,
                ApkName,
                BuildTarget.Android,
                BuildOptions.None);

            Debug.Log("\n\n...Android Build DONE!\n\n");
        }

        private static BuildConfig GetBuildConfig()
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
