namespace _Source.Services.Environment
{
    public class EnvironmentService : IEnvironmentService
    {
        public static bool IsDebugStatic => UnityEngine.Debug.isDebugBuild;
        public bool IsDebug => IsDebugStatic;

        public EnvironmentService()
        {

        }
    }
}
