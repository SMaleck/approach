using System;

namespace Assets.Editor.CiBuild.Configs
{
    [Serializable]
    public class ProjectConfig
    {
        public const string FileName = "project_config.json";

        public string AndroidBuildName;
        public string WebGlBuildName;
    }
}
