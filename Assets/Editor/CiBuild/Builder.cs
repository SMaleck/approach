using Assets.Editor.CiBuild.Config;
using UnityEditor;
using UnityEditor.Build.Reporting;
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

            var environmentConfig = BuildConfig.ReadEnvironmentConfig();
            EditorPrefs.SetString("AndroidSdkRoot", environmentConfig.AndroidSdkRoot);
            EditorPrefs.SetString("AndroidNdkRoot", environmentConfig.AndroidNdkRoot);
            EditorPrefs.SetString("JdkPath", environmentConfig.JavaRoot);
        }

        private static void RunBuild()
        {
            LogHeader("Starting BuildPipeline for Android");

            var buildOptions = BuildConfig.GetBuildPlayerOptions();
            var buildReport = BuildPipeline.BuildPlayer(buildOptions);

            LogHeader("Android Build DONE!");
            LogBuildReport(buildReport);
        }

        private static void LogBuildReport(BuildReport buildReport)
        {
            var exitCode = buildReport.summary.result;
            LogHeader($"BuildReport Result: {exitCode.ToString()}");
        }

        private static void LogHeader(object payload)
        {
            Debug.Log($"\n\n----------------- {payload}\n");
        }
    }
}
