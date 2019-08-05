using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace _Source.App
{
    public static class Logger
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

#if UNITY_EDITOR       

        public static void Log(
            object payload = null,
            [CallerFilePath] string sourceFilePath = "")
        {
            UnityEngine.Debug.Log(BuildMessage(payload, sourceFilePath));
        }

        public static void Warn(
            object payload = null,
            [CallerFilePath] string sourceFilePath = "")
        {
            UnityEngine.Debug.LogWarning(BuildMessage(payload, sourceFilePath));
        }
        
        public static void Error(
            object payload = null,
            [CallerFilePath] string sourceFilePath = "")
        {
            UnityEngine.Debug.LogError(BuildMessage(payload, sourceFilePath));
        }

#endif


#if !UNITY_EDITOR
        
        public static void Log(object payload = null, [CallerFilePath] string sourceFilePath = "") { }
        public static void Warn(object payload = null, [CallerFilePath] string sourceFilePath = "") { }
        public static void Error(object payload = null, [CallerFilePath] string sourceFilePath = "") { }

#endif
    }
}
