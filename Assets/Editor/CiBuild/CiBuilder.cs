using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
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

        private const string AndroidBuildPath = "androidBuild";

        private static AddRequest _installPackagesRequest;

        public static void Run()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Starting Android Build...");
            stringBuilder.AppendLine($"Path: {AndroidBuildPath}");
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
                AndroidBuildPath,
                BuildTarget.Android,
                BuildOptions.None);

            Debug.Log("...Android Build DONE!");
        }
    }
}
