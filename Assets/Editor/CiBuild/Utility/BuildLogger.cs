using System.Linq;
using UnityEngine;

namespace Assets.Editor.CiBuild.Utility
{
    public static class BuildLogger
    {
        private const string HeaderPrefix = "\n\n---------------- ";
        private const string HeaderSuffix = "\n";
        private const string Separator = "--------------------";

        static BuildLogger()
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        }

        public static void Log(object message)
        {
            Debug.Log(message.ToString());
        }

        public static void LogHeader(object message)
        {
            Debug.Log($"{HeaderPrefix}{message}{HeaderSuffix}");
        }

        public static void LogSeparator(object message = null)
        {
            Debug.Log($"{Separator} {message ?? string.Empty} {Separator}");
        }

        public static void LogArray(string label,  string[] items)
        {
            Log(label);
            items.ToList().ForEach(Log);
        }
    }
}
