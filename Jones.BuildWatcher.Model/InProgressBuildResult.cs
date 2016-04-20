using System;

namespace Jones.BuildWatcher.Model
{
    public sealed class InProgressBuildResult : BuildResult
    {
        private TimeSpan _buildTime;
        private bool _previousBuildSucceeded;

        public InProgressBuildResult(string projectName, string buildName, string friendlyName) : base(projectName, buildName, friendlyName) {}

        public TimeSpan BuildTime
        {
            get { return _buildTime; }
            set { SetProperty(ref _buildTime, value); }
        }

        public bool PreviousBuildSucceeded
        {
            get { return _previousBuildSucceeded; }
            set { SetProperty(ref _previousBuildSucceeded, value); }
        }
    }
}