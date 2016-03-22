using System;

namespace Jones.BuildWatcher
{
    public sealed class BuildModel : NotifyBase
    {
        private string _buildName;
        private DateTime? _lastCompleted;
        private string _personName;
        private bool _success;

        public string BuildName
        {
            get { return _buildName; }
            set { SetProperty(nameof(BuildName), ref _buildName, value); }
        }

        public DateTime? LastCompleted
        {
            get { return _lastCompleted; }
            set { SetProperty(nameof(LastCompleted), ref _lastCompleted, value); }
        }

        public string PersonName
        {
            get { return _personName; }
            set { SetProperty(nameof(PersonName), ref _personName, value); }
        }

        public bool Success
        {
            get { return _success; }
            set { SetProperty(nameof(Success), ref _success, value); }
        }
    }
}