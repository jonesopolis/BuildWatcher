using Jones.Utilities;

namespace Jones.BuildWatcher.Model
{
    public abstract class BuildResult : NotifyBase
    {
        private string _buildName;
        private string _friendlyName;
        private string _projectName;
        private string _personName;

        protected BuildResult(string projectName, string buildName, string friendlyName)
        {
            ProjectName = projectName;
            BuildName = buildName;
            FriendlyName = friendlyName;
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

        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }

        public string PersonName
        {
            get { return _personName; }
            set { SetProperty(ref _personName, value); }
        }
    }
}