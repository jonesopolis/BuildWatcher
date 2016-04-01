using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Jones.BuildWatcher.Model;
using Jones.BuildWatcher.Repository;
using Jones.Utilities;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;

namespace Jones.BuildWatcher
{
    public sealed class BuildVM : NotifyBase
    {
        private readonly IBuildRepository _buildRepo;
        private readonly LiveConfig<IEnumerable<BuildConfiguration>> _liveConfig;
        private readonly Uri _uri = new Uri("http://tfs.csiweb.com:8080/tfs/DefaultCollection");
        private Queue<Build> _queue;
        private Timer _timer;

        public BuildVM(IBuildRepository buildRepo)
        {
            if (buildRepo == null)
            {
                throw new ArgumentNullException(nameof(buildRepo));
            }

            _buildRepo = buildRepo;

            try
            {
                _liveConfig = new LiveConfig<IEnumerable<BuildConfiguration>>("Configuration/config.json");
                _liveConfig.Changed += initializeItems;
                _liveConfig.Unavailable += () => { };
                _liveConfig.Watch();
                

                _timer = new Timer();
                _timer.Interval = 1000;
                _timer.Elapsed += poll;
                _timer.Start();
            }
            catch (Exception ex)
            {
                //TODO
            }
        }

        public ObservableCollection<Build> Items { get; } = new ObservableCollection<Build>();

        private void initializeItems()
        {
            foreach (var build in _liveConfig.Configuration.SelectMany(c => c.Builds))
            {
                var model = new Build();
                model.BuildName = build.Value;
                model.PersonName = "N/A";
                model.IsGreen = false;
            }
        }

        private void poll(object sender, ElapsedEventArgs elapsedEventArgs)
    {
            if (_liveConfig.Configuration == null)
            {
                return;
            }

            foreach (var item in _config)
            {
                Trace.WriteLine(item.ProjectName);
                Trace.WriteLine("\t" + string.Join(", ", item.Builds.Select(b => b.Value)));
            }

            Console.WriteLine();
        }
    }
}