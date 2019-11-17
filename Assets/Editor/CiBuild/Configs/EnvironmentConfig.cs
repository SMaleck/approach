using System;

namespace Assets.Editor.CiBuild.Configs
{
    [Serializable]
    public class EnvironmentConfig
    {
        public const string FileName = "environment_config.json";

        public string AndroidSdkRoot;
        public string AndroidNdkRoot;
        public string JavaRoot;
    }
}
