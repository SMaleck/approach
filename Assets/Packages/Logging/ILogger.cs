using System.Runtime.CompilerServices;

namespace Packages.Logging
{
    public interface ILogger
    {
        void Log(object payload = null, [CallerFilePath] string sourceFilePath = "");
        void Warn(object payload = null, [CallerFilePath] string sourceFilePath = "");
        void Error(object payload = null, [CallerFilePath] string sourceFilePath = "");
    }
}
