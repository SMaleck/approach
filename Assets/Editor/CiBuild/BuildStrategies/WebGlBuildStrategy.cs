using Assets.Editor.CiBuild.Utility;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Assets.Editor.CiBuild.BuildStrategies
{
    public static class WebGlBuildStrategy
    {
        private const string BuildArtifactPath = "/builds/approach_webGl/";

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

            var scenePaths = BuildUtils.GetScenePaths();

            BuildLogger.Log($"BuildArtifactPath: {BuildArtifactPath}");
            BuildLogger.LogArray("Scenes:", scenePaths);

            return new BuildPlayerOptions()
            {
                scenes = scenePaths,
                locationPathName = BuildArtifactPath,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };
        }
    }
}
