using Assets.Editor.CiBuild.Configs;
using Assets.Editor.CiBuild.Utility;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Assets.Editor.CiBuild.BuildStrategies
{
    public static class AndroidBuildStrategy
    {
        private const string ApkName = "/builds/approach.apk";

        public static BuildReport RunBuild()
        {
            BuildLogger.LogHeader("Starting BuildPipeline for ANDROID");

            Setup();

            var buildOptions = GetBuildPlayerOptions();
            return BuildPipeline.BuildPlayer(buildOptions);
        }

        private static void Setup()
        {
            BuildLogger.LogHeader("Setting up build environment");

            var environmentConfig = BuildConfig.ReadEnvironmentConfig();
            EditorPrefs.SetString("AndroidSdkRoot", environmentConfig.AndroidSdkRoot);
            EditorPrefs.SetString("AndroidNdkRoot", environmentConfig.AndroidNdkRoot);
            EditorPrefs.SetString("JdkPath", environmentConfig.JavaRoot);
        }

        private static BuildPlayerOptions GetBuildPlayerOptions()
        {
            BuildLogger.LogSeparator("Generating Build Options");

            var scenePaths = BuildUtils.GetScenePaths();

            BuildLogger.Log($"APK Name: {ApkName}");
            BuildLogger.LogArray("Scenes:", scenePaths);

            return new BuildPlayerOptions()
            {
                scenes = scenePaths,
                locationPathName = ApkName,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };
        }
    }
}
