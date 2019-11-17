using Assets.Editor.CiBuild.Configs;
using Assets.Editor.CiBuild.Utility;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Assets.Editor.CiBuild.BuildStrategies
{
    public static class WebGlBuildStrategy
    {
        public static BuildReport RunBuild()
        {
            BuildLogger.LogHeader("Starting BuildPipeline for WEBGL");

            var buildOptions = GetBuildPlayerOptions();

            BuildLogger.LogSeparator("Running BuildPlayer");

            return BuildPipeline.BuildPlayer(buildOptions);
        }

        private static BuildPlayerOptions GetBuildPlayerOptions()
        {
            BuildLogger.LogSeparator("Generating Build Options");

            var projectConfig = BuildConfig.ReadProjectConfig();
            var scenePaths = BuildUtils.GetScenePaths();

            BuildLogger.Log($"BuildArtifactPath: {projectConfig.WebGlBuildName}");
            BuildLogger.LogArray("Scenes:", scenePaths);

            return new BuildPlayerOptions()
            {
                scenes = scenePaths,
                locationPathName = projectConfig.WebGlBuildName,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };
        }
    }
}
