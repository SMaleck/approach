using Assets.Editor.CiBuild.BuildStrategies;
using Assets.Editor.CiBuild.Utility;
using System;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace Assets.Editor.CiBuild
{
    public static class Builder
    {
        public static void Run()
        {
            BuildLogger.LogHeader("Starting Build Script");

            BuildReport buildReport = null;
            var buildTarget = BuildUtils.GetBuildTarget();

            switch (buildTarget)
            {
                case BuildTarget.NoTarget:
                    EditorApplication.Exit(-1);
                    break;

                case BuildTarget.WebGL:
                    buildReport = WebGlBuildStrategy.RunBuild();
                    break;

                case BuildTarget.Android:
                    buildReport = AndroidBuildStrategy.RunBuild();
                    break;

                default:
                    throw new ArgumentException($"BuildTarget {buildTarget} is not supported");
            }

            ProcessBuildReport(buildReport);
        }

        private static void ProcessBuildReport(BuildReport buildReport)
        {
            BuildLogger.LogHeader("BUILD COMPLETED!");

            var exitCode = buildReport.summary.result;
            BuildLogger.LogHeader($"BuildReport Result: {exitCode.ToString()}");
        }
    }
}
