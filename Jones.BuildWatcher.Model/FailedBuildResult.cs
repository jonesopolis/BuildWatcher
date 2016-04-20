using System;
using Jones.Utilities;

namespace Jones.BuildWatcher.Model
{
    public sealed class FailedBuildResult : BuildResult
    {
        private DateTime _failed;

        public DateTime Failed
        {
            get { return _failed; }
            set { SetProperty(ref _failed, value); }
        }

        public FailedBuildResult(string projectName, string buildName, string friendlyName) : base(projectName, buildName, friendlyName) {}
    }
}