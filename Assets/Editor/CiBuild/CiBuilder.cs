using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private const string AndroidBuildPath = "androidBuild";

        public static void Run()
        {
            Debug.Log("Starting Android Build...");

            Debug.Log($"Path: {AndroidBuildPath}");
            Debug.Log("Scenes:");
            foreach (var scene in BuildScenes)
            {
                Debug.Log(scene);
            }

            BuildPipeline.BuildPlayer(
                BuildScenes, 
                AndroidBuildPath, 
                BuildTarget.Android, 
                BuildOptions.None);
            
            Debug.Log("...Android Build DONE!");
            EditorApplication.Exit(0);
        }
    }
}
