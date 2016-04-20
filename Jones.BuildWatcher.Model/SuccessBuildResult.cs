using System;

namespace Jones.BuildWatcher.Model
{
    public sealed class SuccessBuildResult : BuildResult
    {
        private DateTime _completed;

        public SuccessBuildResult(string projectName, string buildName, string friendlyName) : base(projectName, buildName, friendlyName) {}

        public DateTime Completed
        {
            get { return _completed; }
            set { SetProperty(ref _completed, value); }
        }
    }
}