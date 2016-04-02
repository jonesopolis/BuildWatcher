using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using Jones.BuildWatcher.Model;
using Jones.BuildWatcher.Repository;
using Jones.Utilities;

namespace Jones.BuildWatcher
{
    public sealed class BuildVM : NotifyBase
    {
        private readonly IBuildRepository _buildRepo;
        private readonly LiveConfig<List<BuildConfiguration>> _liveConfig;
        private readonly Queue<Tuple<string,string>> _queue;
        private readonly Timer _timer;
        private Build _currentBuild;

        public BuildVM(IBuildRepository buildRepo)
        {
            if (buildRepo == null)
            {
                throw new ArgumentNullException(nameof(buildRepo));
            }

            _buildRepo = buildRepo;
            _queue = new Queue<Tuple<string, string>>();
            Items = new ObservableCollection<Build>();

            try
            {
                _liveConfig = new LiveConfig<List<BuildConfiguration>>("Configuration/config.json");
                _liveConfig.Changed += initializeItems;
                _liveConfig.Unavailable += () => { };
                _liveConfig.Watch();

                _timer = new Timer();
                _timer.Interval = 2000;
                _timer.Elapsed += change;
                _timer.Start();
            }
            catch (Exception ex)
            {
                //TODO
            }
        }

        public Build CurrentBuild
        {
            get { return _currentBuild; }
            set { SetProperty(ref _currentBuild, value); }
        }

        public ObservableCollection<Build> Items { get; } 

        private void change(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            var item = _queue.Dequeue();
            
            CurrentBuild = _buildRepo.GetBuildResults(item.Item1, item.Item2);

            _queue.Enqueue(item);
        }

        private void initializeItems()
        {
            _queue.Clear();

            foreach (var project in _liveConfig.Configuration)
            {
                foreach (var build in project.Builds)
                {
                    _queue.Enqueue(new Tuple<string, string>(project.ProjectName, build.Key));   
                }
            }
        }
    }
}