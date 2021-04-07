using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Packages.Logging
{
    public static class StaticLogger
    {
        private static string BuildMessage(object payload = null, string sourceFilePath = "")
        {
            var message = payload?.ToString() ?? string.Empty;
            var className = sourceFilePath
                .Split(Path.DirectorySeparatorChar)
                .LastOrDefault()
                ?.Replace(".cs", "");

            return $"[{className}] {message}";
        }

        [Conditional("UNITY_EDITOR")]
        public static void Log(
            object payload = null,
            [CallerFilePath] string sourceFilePath = "")
        {
            UnityEngine.Debug.Log(BuildMessage(payload, sourceFilePath));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warn(
            object payload = null,
            [CallerFilePath] string sourceFilePath = "")
        {
            UnityEngine.Debug.LogWarning(BuildMessage(payload, sourceFilePath));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Error(
            object payload = null,
            [CallerFilePath] string sourceFilePath = "")
        {
            UnityEngine.Debug.LogError(BuildMessage(payload, sourceFilePath));
        }
    }
}