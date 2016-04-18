using System;
using Jones.Utilities;

namespace Jones.BuildWatcher.Model
{
    public sealed class Build : NotifyBase
    {
        private string _buildName;
        private DateTime? _lastCompleted;
        private string _personName;
        private bool _isGreen;
        private string _friendlyName;
        private string _projectName;

        public Build(string projectName, string buildName, string friendlyName) : this(projectName, buildName)
        {
            FriendlyName = friendlyName;
        }

        public Build(string projectName, string buildName) 
        {
            ProjectName = projectName;
            BuildName = buildName;
        }

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

        public DateTime? LastCompleted
        {
            get { return _lastCompleted; }
            set { SetProperty(ref _lastCompleted, value); }
        }

        public string PersonName
        {
            get { return _personName; }
            set { SetProperty(ref _personName, value); }
        }

        public bool IsGreen
        {
            get { return _isGreen; }
            set { SetProperty(ref _isGreen, value); }
        }
    }
}