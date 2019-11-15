using System;
using System.Linq;
using UnityEditor;

namespace Assets.Editor.CiBuild.Utility
{
    public static class BuildUtils
    {
        private const string BuildTargetArgKey = "--buildTarget";

        public static BuildTarget GetBuildTarget()
        {
            var args = System.Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i].Equals(BuildTargetArgKey))
                {
                    var buildTargetArgPayload = args[i + 1];
                    return (BuildTarget)Enum.Parse(typeof(BuildTarget), buildTargetArgPayload);
                }
            }

            return BuildTarget.NoTarget;
        }

        public static string[] GetScenePaths()
        {
            return EditorBuildSettings.scenes
                .ToList()
                .Select(scene => scene.path)
                .ToArray();
        }
    }
}
