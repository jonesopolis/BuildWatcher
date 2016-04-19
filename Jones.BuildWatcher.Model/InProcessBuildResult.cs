using Jones.Utilities;

namespace Jones.BuildWatcher.Model
{
    public sealed class InProcessBuildResult : NotifyBase, IBuildResult
    {
        public string ProjectName { get; set; }
        public string BuildName { get; set; }
        public string FriendlyName { get; set; }
    }
}