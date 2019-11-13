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

        public static void Run()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\n\n----------- Starting Build Script\n");
            stringBuilder.AppendLine("Starting Android Build...");
            stringBuilder.AppendLine($"APK Path: {ApkName}");
            stringBuilder.AppendLine("Scenes:");
            foreach (var scene in BuildScenes)
            {
                stringBuilder.AppendLine(scene);
            }

            stringBuilder.AppendLine("----------- -----------");
            Debug.Log(stringBuilder.ToString());

            SetAndroidEnvironment();
            RunBuild();
        }

        private static void SetAndroidEnvironment()
        {
            Debug.Log("\n----------- Setting Android Environment");

            EditorPrefs.SetString("AndroidSdkRoot", "/opt/tools/android");
            EditorPrefs.SetString("AndroidNdkRoot", "/opt/ndk");
        }

        private static void RunBuild()
        {
            Debug.Log("\n----------- Starting BuildPipeline");

            BuildPipeline.BuildPlayer(
                BuildScenes,
                ApkName,
                BuildTarget.Android,
                BuildOptions.None);

            Debug.Log("\n\n...Android Build DONE!\n\n");
        }
    }
}
