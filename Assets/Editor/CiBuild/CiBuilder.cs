using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CiBuild
{
    public static class CiBuilder
    {
        private static readonly string[] BuildScenes =
        {
            "Assets/InitScene.unity",
            "Assets/TitleScene.unity" ,
            "Assets/GameScene.unity"
        };

        private const string ApkName = "approach.apk";
        private const string AndroidBuildPath = "androidBuild";

        private const string AndroidSdkRoot = "/opt/platform-tools/";
        private const string AndroidNdkRoot = "/opt/ndk/";

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

            EditorPrefs.SetString("AndroidSdkRoot", AndroidSdkRoot);
            EditorPrefs.SetString("AndroidNdkRoot", AndroidNdkRoot);
        }

        private static void LogSetup()
        {
            Debug.Log($"APK Path: {ApkName}");
            Debug.Log("Scenes:");
            foreach (var scene in BuildScenes)
            {
                Debug.Log(scene);
            }

            Debug.Log($"AndroidSdkRoot: {AndroidSdkRoot}");
            Debug.Log($"AndroidNdkRoot: {AndroidNdkRoot}");
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
    }
}
