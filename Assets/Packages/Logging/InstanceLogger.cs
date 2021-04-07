using System.Runtime.CompilerServices;

namespace Packages.Logging
{
    public class InstanceLogger : ILogger
    {
        public void Log(object payload = null, [CallerFilePath] string sourceFilePath = "")
        {
            StaticLogger.Log(payload, sourceFilePath);
        }

        public void Warn(object payload = null, [CallerFilePath] string sourceFilePath = "")
        {
            StaticLogger.Warn(payload, sourceFilePath);
        }

        public void Error(object payload = null, [CallerFilePath] string sourceFilePath = "")
        {
            StaticLogger.Error(payload, sourceFilePath);
        }
    }
}
