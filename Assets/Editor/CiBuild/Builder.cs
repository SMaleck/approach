using Assets.Editor.CiBuild.Config;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CiBuild
{
    public static class Builder
    {
        private static BuildConfig BuildConfig;

        private static readonly string[] BuildScenes =
        {
            "Assets/InitScene.unity",
            "Assets/TitleScene.unity" ,
            "Assets/GameScene.unity"
        };

        private const string ApkName = "approach.apk";
        private const string AndroidBuildPath = "androidBuild";

        static Builder()
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        }

        public static void Run()
        {
            LogHeader("Starting Build Script");

            Setup();
            LogSetup();
            RunBuild();
        }

        private static void Setup()
        {
            LogHeader("Preparing Environment");

            BuildConfig = BuildConfigReader.Read();
            EditorPrefs.SetString("AndroidSdkRoot", BuildConfig.AndroidSdkRoot);
            EditorPrefs.SetString("AndroidNdkRoot", BuildConfig.AndroidNdkRoot);
            EditorPrefs.SetString("JdkPath", BuildConfig.JavaRoot);
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
            LogHeader("Starting BuildPipeline for Android");

            BuildPipeline.BuildPlayer(
                BuildScenes,
                ApkName,
                BuildTarget.Android,
                BuildOptions.None);

            LogHeader("Android Build DONE!");
        }

        private static void LogHeader(object payload)
        {
            Debug.Log($"\n\n----------------- {payload}\n");
        }
    }
}
