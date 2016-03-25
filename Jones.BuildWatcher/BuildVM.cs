using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Jones.Utilities;

//using Microsoft.TeamFoundation.Build.Client;
//using Microsoft.TeamFoundation.Client;

namespace Jones.BuildWatcher
{
    public sealed class BuildVM : NotifyBase
    {
        private readonly LiveConfig<IEnumerable<BuildConfiguration>> _liveConfig;
        private readonly Uri _uri = new Uri("http://tfs.csiweb.com:8080/tfs/DefaultCollection");
        private IEnumerable<BuildConfiguration> _config;
        private Timer _timer;
        //private readonly TfsTeamProjectCollection _tfs;

        public BuildVM()
        {
            try
            {
                _liveConfig = new LiveConfig<IEnumerable<BuildConfiguration>>("Configuration/config.json");
                _liveConfig.Changed += initializeItems;
                _liveConfig.Unavailable += () => _config = null;
                _liveConfig.Watch();

                //_tfs = new TfsTeamProjectCollection(_uri);


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

        public ObservableCollection<BuildModel> Items { get; } = new ObservableCollection<BuildModel>();

        private void initializeItems()
        {
            _config = _liveConfig.Configuration;

            foreach (var build in _config.SelectMany(c => c.Builds))
            {
                var model = new BuildModel();
                model.BuildName = build.Value;
                model.PersonName = "N/A";
                model.Success = false;
            }
        }

        private void poll(object sender, ElapsedEventArgs elapsedEventArgs)
    {
            if (_config == null)
            {
                return;
            }

            foreach (var item in _config)
            {
                Trace.WriteLine(item.ProjectName);
                Trace.WriteLine("\t" + string.Join(", ", item.Builds.Select(b => b.Value)));
            }

            Console.WriteLine();

            //var buildServer = _tfs.GetService<IBuildServer>();

            //foreach (var configProject in _config)
            //{
            //    foreach (var configBuild in configProject.Builds)
            //    {
            //        var spec = buildServer.CreateBuildDetailSpec(configProject.ProjectName, configBuild.Key);
            //        spec.MaxBuildsPerDefinition = 1;
            //        spec.QueryOrder = BuildQueryOrder.FinishTimeDescending;

            //        var build = buildServer.QueryBuilds(spec);

            //        if (build.Builds.Any())
            //        {
            //            var buildDetail = build.Builds[0];
            //            Trace.WriteLine($"{configBuild.Value} - {buildDetail.Status} - {buildDetail.FinishTime} - {buildDetail.RequestedFor}");
            //        }
            //    }
            //}
        }
    }
}