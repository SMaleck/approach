using Assets.Editor.CiBuild.Config;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CiBuild
{
    public static class Builder
    {
        static Builder()
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        }

        public static void Run()
        {
            LogHeader("Starting Build Script");

            Setup();
            RunBuild();
        }

        private static void Setup()
        {
            LogHeader("Preparing Environment");

            var environmentConfig = Config.BuildConfig.ReadEnvironmentConfig();
            EditorPrefs.SetString("AndroidSdkRoot", environmentConfig.AndroidSdkRoot);
            EditorPrefs.SetString("AndroidNdkRoot", environmentConfig.AndroidNdkRoot);
            EditorPrefs.SetString("JdkPath", environmentConfig.JavaRoot);
        }

        private static void RunBuild()
        {
            LogHeader("Starting BuildPipeline for Android");

            var buildOptions = BuildConfig.GetBuildPlayerOptions();
            Debug.Log(buildOptions);

            BuildPipeline.BuildPlayer(buildOptions);
            LogHeader("Android Build DONE!");
        }

        private static void LogHeader(object payload)
        {
            Debug.Log($"\n\n----------------- {payload}\n");
        }
    }
}
