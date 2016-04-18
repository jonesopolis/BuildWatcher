using System;
using Jones.Utilities;

namespace Jones.BuildWatcher.Model
{
    public sealed class FailedBuildResult : NotifyBase, IBuildResult
    {
        private string _buildName;
        private DateTime _timeFailed;
        private string _personName;
        private string _friendlyName;
        private string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }

        public string BuildName
        {
            get { return _buildName; }
            set { SetProperty(ref _buildName, value); }
        }

        public string FriendlyName
        {
            get { return _friendlyName; }
            set { SetProperty(ref _friendlyName, value); }
        }

        public DateTime TimeFailed
        {
            get { return _timeFailed; }
            set { SetProperty(ref _timeFailed, value); }
        }

        public string PersonName
        {
            get { return _personName; }
            set { SetProperty(ref _personName, value); }
        }
    }
}