using System.Collections.Generic;

namespace Jones.BuildWatcher
{
    public sealed class BuildConfiguration
    {
        public string ProjectName { get; set; }
        public Dictionary<string, string> Builds { get; set; }
    }
}