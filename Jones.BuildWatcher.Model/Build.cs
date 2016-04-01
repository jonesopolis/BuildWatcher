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

        public string BuildName
        {
            get { return _buildName; }
            set { SetProperty(ref _buildName, value); }
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