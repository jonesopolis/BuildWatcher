using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Timers;
using Jones.BuildWatcher.Model;
using Jones.BuildWatcher.Repository;
using Jones.Logger;
using Jones.Utilities;

namespace Jones.BuildWatcher
{
    public sealed class BuildVM : NotifyBase
    {
        private readonly IBuildRepository _buildRepo;
        private readonly Logger.Logger _logger;
        private readonly LiveConfig<List<BuildConfig>> _liveConfig;
        private int  _currentIndex;
        private DateTime _currentTime;

        public BuildVM(IBuildRepository buildRepo, Logger.Logger logger)
        {
            if (buildRepo == null)
            {
                throw new ArgumentNullException(nameof(buildRepo));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _buildRepo = buildRepo;
            _logger = logger;
            Items = new ObservableCollection<BuildResult>();

            try
            {
                _liveConfig = new LiveConfig<List<BuildConfig>>("config.json");
                _liveConfig.Changed += initializeItems;
                _liveConfig.Unavailable += () =>
                {
                    Items.Clear();
                    _currentIndex = 0;
                };
                _liveConfig.Watch();

                var timer = new Timer();
                timer.Interval = 2000;
                timer.Elapsed += change;
                timer.Start();

                var dateTimer = new Timer();
                dateTimer.Interval = 1000;
                dateTimer.Elapsed += (s, e) => CurrentTime = DateTime.Now;
                dateTimer.Start();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Info, ex);
            }
        }
        
        public DateTime CurrentTime
        {
            get { return _currentTime; }
            set { SetProperty(ref _currentTime, value); }
        }

        public ObservableCollection<BuildResult> Items { get; } 

        private void change(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (!Items.Any())
            {
                return;
            }

            if (_currentIndex == Items.Count)
            {
                _currentIndex = 0;
            }
            
            Items[_currentIndex] = _buildRepo.GetSingleBuild(Items[_currentIndex].ProjectName, Items[_currentIndex].BuildName, Items[_currentIndex].FriendlyName);

            ++_currentIndex;
        }

        private void initializeItems()
        {
            Items.Clear();
            _currentIndex = 0;

            foreach (var item in _liveConfig.Configuration)
            {
                Items.Add(new UnknownBuildResult(item.Project, item.Build, item.FriendlyName));                   
            }
        }
    }
}