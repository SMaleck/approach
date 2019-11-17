using Assets.Editor.CiBuild.Configs;
using Assets.Editor.CiBuild.Utility;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Assets.Editor.CiBuild.BuildStrategies
{
    public static class AndroidBuildStrategy
    {
        public static BuildReport RunBuild()
        {
            BuildLogger.LogSeparator("Starting BuildPipeline for ANDROID");

            Setup();

            var buildOptions = GetBuildPlayerOptions();

            BuildLogger.LogSeparator("Running BuildPlayer");

            return BuildPipeline.BuildPlayer(buildOptions);
        }

        private static void Setup()
        {
            BuildLogger.LogSeparator("Setting up build environment");

            var environmentConfig = BuildConfig.ReadEnvironmentConfig();
            EditorPrefs.SetString("AndroidSdkRoot", environmentConfig.AndroidSdkRoot);
            EditorPrefs.SetString("AndroidNdkRoot", environmentConfig.AndroidNdkRoot);
            EditorPrefs.SetString("JdkPath", environmentConfig.JavaRoot);
        }

        private static BuildPlayerOptions GetBuildPlayerOptions()
        {
            BuildLogger.LogSeparator("Generating Build Options");

            var projectConfig = BuildConfig.ReadProjectConfig();
            var scenePaths = BuildUtils.GetScenePaths();

            BuildLogger.Log($"APK Name: {projectConfig.AndroidBuildName}");
            BuildLogger.LogArray("Scenes:", scenePaths);

            return new BuildPlayerOptions()
            {
                scenes = scenePaths,
                locationPathName = projectConfig.AndroidBuildName,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };
        }
    }
}
