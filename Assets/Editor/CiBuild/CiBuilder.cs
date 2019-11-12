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
            stringBuilder.AppendLine("\n\n----------- BUILD SCRIPT -----------\n");
            stringBuilder.AppendLine("Starting Android Build...");
            stringBuilder.AppendLine($"APK Path: {ApkName}");
            stringBuilder.AppendLine("Scenes:");
            foreach (var scene in BuildScenes)
            {
                stringBuilder.AppendLine(scene);
            }

            Debug.Log(stringBuilder.ToString());

            RunBuild();
        }

        private static void RunBuild()
        {
            BuildPipeline.BuildPlayer(
                BuildScenes,
                ApkName,
                BuildTarget.Android,
                BuildOptions.None);

            Debug.Log("\n\n...Android Build DONE!");
        }
    }
}
